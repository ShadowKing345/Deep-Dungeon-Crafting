using System.Collections.Generic;
using System.Linq;
using Project.Runtime.Crafting;
using Project.Runtime.Entity.Player;
using Project.Runtime.Enums;
using Project.Runtime.Managers;
using Project.Runtime.Ui.Inventories.ItemSlot;
using Project.Runtime.Utils.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Runtime.Ui.Crafting
{
    public class CraftingController : MonoBehaviour, IUiWindow
    {
        [Header("OtherObj.")] [SerializeField] private PlayerMovement playerMovementController;

        [SerializeField] private PlayerCombat playerCombatController;

        [Space] [Header("Crafting Page")] [SerializeField]
        private ItemStackSlot resultItemSlot;

        [SerializeField] private GameObject ingredientsContent;
        [SerializeField] private GameObject ingredientItemSlotPreFab;
        [SerializeField] private Button craftButton;

        [Space] [Header("Navigation")] [SerializeField]
        private Recipe selectedRecipe;

        [SerializeField] private Transform navigationContent;
        [SerializeField] private GameObject recipeEntryPreFab;

        [Space] [Header("Inventory")] [SerializeField]
        private PlayerInventory playerInventory;

        private readonly List<ItemStackSlot> ingredientsItemSlots = new();
        private readonly List<GameObject> recipeEntries = new();
        private CraftingManager _craftingManager;
        private UiManager _uiManager;

        private void Awake()
        {
            _craftingManager = CraftingManager.Instance;
            _uiManager = UiManager.Instance;

            _uiManager.RegisterWindow(WindowReference.CraftingMenu, gameObject);
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            playerInventory ??= FindObjectOfType<PlayerInventory>();

            playerMovementController ??= FindObjectOfType<PlayerMovement>();
            playerCombatController ??= FindObjectOfType<PlayerCombat>();
        }

        private void OnDisable()
        {
            foreach (Transform child in navigationContent.transform) Destroy(child.gameObject);
        }

        private void OnDestroy()
        {
            _uiManager.UnregisterWindow(WindowReference.CraftingMenu, gameObject);
        }

        public void Show()
        {
            playerMovementController.enabled = playerCombatController.enabled = false;
            CreateRecipeEntries();
            navigationContent.GetComponentInChildren<Selectable>().Select();
        }

        public void Hide()
        {
            playerMovementController.enabled = playerCombatController.enabled = true;
        }

        public void Craft()
        {
            if (selectedRecipe == null) return;

            if (!CraftingManager.CanCraft(playerInventory.ItemInventory, selectedRecipe)) return;

            CraftingManager.Craft(playerInventory.ItemInventory, selectedRecipe);
            UpdatePage(selectedRecipe);
        }

        private void CreateRecipeEntries()
        {
            foreach (var obj in recipeEntries) Destroy(obj);
            recipeEntries.Clear();

            var recipes = new SortedList<string, Recipe>(_craftingManager.Recipes.ToDictionary(s => s.name));

            foreach (var recipe in recipes.Values)
            {
                var obj = Instantiate(recipeEntryPreFab, navigationContent);
                recipeEntries.Add(obj);
                if (!obj.TryGetComponent(out RecipeEntry entry)) continue;
                entry.Recipe = recipe;
                entry.Controller = this;
            }
        }

        public void UpdatePage(Recipe recipe)
        {
            selectedRecipe = recipe;
            resultItemSlot.ItemStack = recipe.Result;

            foreach (var slot in ingredientsItemSlots) Destroy(slot.gameObject);
            ingredientsItemSlots.Clear();

            var ingredients =
                CraftingManager.GetMissingIngredients(playerInventory.ItemInventory, recipe);

            foreach (var kvPair in ingredients)
            {
                var obj = Instantiate(ingredientItemSlotPreFab, ingredientsContent.transform);
                obj.SetActive(true);
                if (!obj.TryGetComponent(out IngredientItemStack slot)) continue;

                slot.ItemStack = kvPair.Key;
                slot.HasItemStack = kvPair.Value;

                ingredientsItemSlots.Add(slot);
            }

            craftButton.interactable = CraftingManager.CanCraft(playerInventory.ItemInventory, recipe);
        }
    }
}
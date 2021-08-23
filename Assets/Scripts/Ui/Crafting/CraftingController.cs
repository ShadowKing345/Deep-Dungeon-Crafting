using System.Collections.Generic;
using System.Linq;
using Crafting;
using Entity.Player;
using Enums;
using Interfaces;
using Items;
using Managers;
using Ui.Inventories.ItemSlot;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Crafting
{
    public class CraftingController : MonoBehaviour, IUiWindow
    {
        private CraftingManager _craftingManager;
        private UiManager _uiManager;
        
        private readonly List<ItemStackSlot> ingredientsItemSlots = new List<ItemStackSlot>(); 
        private readonly List<GameObject> recipeEntries = new List<GameObject>();
     
        [Header("OtherObj.")]
        [SerializeField] private PlayerMovement playerMovementController;
        [SerializeField] private PlayerCombat playerCombatController;
        [Space]        
        [Header("Crafting Page")]
        [SerializeField] private ItemStackSlot resultItemSlot;
        [SerializeField] private GameObject ingredientsContent;
        [SerializeField] private GameObject ingredientItemSlotPreFab;
        [SerializeField] private Button craftButton;
        [Space]
        [Header("Navigation")]
        [SerializeField] private Recipe selectedRecipe;
        [SerializeField] private Transform navigationContent;
        [SerializeField] private GameObject recipeEntryPreFab;
        [Space]
        [Header("Inventory")]
        [SerializeField] private PlayerInventory playerInventory;

        private void Awake()
        {
            _craftingManager = CraftingManager.Instance;
            _uiManager = UiManager.Instance;
            
            _uiManager.RegisterWindow(WindowReference.CraftingMenu, gameObject);
            gameObject.SetActive(false);
        }

        private void OnDestroy() => _uiManager.UnregisterWindow(WindowReference.CraftingMenu, gameObject);

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

        public void Craft()
        {
            if (selectedRecipe == null) return;

            if (!CraftingManager.CanCraft(playerInventory.ItemInventory, selectedRecipe)) return;
            
            CraftingManager.Craft(playerInventory.ItemInventory, selectedRecipe);
            UpdatePage(selectedRecipe);
        }

        private void CreateRecipeEntries()
        {
            foreach(GameObject obj in recipeEntries) Destroy(obj);
            recipeEntries.Clear();

            SortedList<string, Recipe> recipes = new SortedList<string, Recipe>(_craftingManager.Recipes.ToDictionary(s => s.name));

            foreach (Recipe recipe in recipes.Values)
            {
                GameObject obj = Instantiate(recipeEntryPreFab, navigationContent);
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
            
            foreach (ItemStackSlot slot in ingredientsItemSlots) Destroy(slot.gameObject);
            ingredientsItemSlots.Clear();

            Dictionary<ItemStack, bool> ingredients =
                CraftingManager.GetMissingIngredients(playerInventory.ItemInventory, recipe);
            
            foreach (KeyValuePair<ItemStack, bool> kvPair in ingredients)
            {
                GameObject obj = Instantiate(ingredientItemSlotPreFab, ingredientsContent.transform);
                obj.SetActive(true);
                if (!obj.TryGetComponent(out IngredientItemStack slot)) continue;
                
                slot.ItemStack = kvPair.Key;
                slot.HasItemStack = kvPair.Value;
                
                ingredientsItemSlots.Add(slot);
            }

            craftButton.interactable = CraftingManager.CanCraft(playerInventory.ItemInventory, recipe);
        }
        
        public void Show()
        {
            playerMovementController.enabled = playerCombatController.enabled = false;
            CreateRecipeEntries();
            navigationContent.GetComponentInChildren<Selectable>().Select();
        }

        public void Hide() => playerMovementController.enabled = playerCombatController.enabled = true;
    }
}
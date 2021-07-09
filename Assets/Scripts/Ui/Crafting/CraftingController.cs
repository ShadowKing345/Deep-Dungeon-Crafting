using System.Collections.Generic;
using Crafting;
using Entity.Player;
using Items;
using Ui.Inventories.ItemSlot;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Crafting
{
    public class CraftingController : MonoBehaviour
    {
        private CraftingManager _craftingManager;
        private readonly List<ItemStackSlot> ingredientsItemSlots = new List<ItemStackSlot>(); 
        private readonly List<GameObject> recipeEntries = new List<GameObject>();
        
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

        private void OnEnable()
        {
            _craftingManager = CraftingManager.Instance;

            playerInventory ??= FindObjectOfType<PlayerInventory>();
            ingredientItemSlotPreFab.SetActive(false);
        }

        private void OnDisable()
        {
            foreach (Transform child in navigationContent.transform) Destroy(child.gameObject);
        }

        public void Craft()
        {
            if (selectedRecipe == null) return;

            if (!_craftingManager.CanCraft(playerInventory.ItemInventory, selectedRecipe)) return;
            
            _craftingManager.Craft(playerInventory.ItemInventory, selectedRecipe);
            UpdatePage(selectedRecipe);
        }

        public void CreateRecipeEntries()
        {
            foreach(GameObject obj in recipeEntries) Destroy(obj);
            recipeEntries.Clear();

            foreach (Recipe recipe in _craftingManager.Recipes)
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
                _craftingManager.GetMissingIngredients(playerInventory.ItemInventory, recipe);
            
            foreach (KeyValuePair<ItemStack, bool> kvPair in ingredients)
            {
                GameObject obj = Instantiate(ingredientItemSlotPreFab, ingredientsContent.transform);
                obj.SetActive(true);
                if (!obj.TryGetComponent(out IngredientItemStack slot)) continue;
                
                slot.ItemStack = kvPair.Key;
                slot.HasItemStack = kvPair.Value;
                
                ingredientsItemSlots.Add(slot);
            }

            craftButton.interactable = _craftingManager.CanCraft(playerInventory.ItemInventory, recipe);
        }
    }
}
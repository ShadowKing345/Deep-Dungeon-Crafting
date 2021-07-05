using System.Collections.Generic;
using Crafting;
using Entity.Player;
using Items;
using Ui.Inventories.ItemSlot;
using UnityEngine;

namespace Ui.Crafting
{
    public class CraftingController : MonoBehaviour
    {
        private CraftingManager _craftingManager;
        
        [SerializeField] private PhantomItemSlot resultItemSlot;
        [SerializeField] private GameObject ingredientsContent;
        [SerializeField] private GameObject phantomItemSlotPreFab;
        
        private readonly List<PhantomItemSlot> ingredientsItemSlots = new List<PhantomItemSlot>(); 
        
        [SerializeField] private Recipe selectedRecipe;
        [SerializeField] private Transform navigationContent;
        [SerializeField] private GameObject recipeEntryPreFab;
        private readonly List<GameObject> recipeEntries = new List<GameObject>();

        [SerializeField] private PlayerInventory playerInventory;

        private void Start()
        {
            _craftingManager = CraftingManager.instance;
            
            CreateRecipeEntries();
            playerInventory ??= FindObjectOfType<PlayerInventory>();
        }

        public void Craft()
        {
            if (selectedRecipe == null) return;

            if (_craftingManager.CanCraft(playerInventory.ItemInventory, selectedRecipe)) _craftingManager.Craft(playerInventory.ItemInventory, selectedRecipe);
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

        public void UpdateRecipe(Recipe recipe)
        {
            selectedRecipe = recipe;
            resultItemSlot.ItemStack = recipe.Result;
            
            foreach (PhantomItemSlot slot in ingredientsItemSlots) Destroy(slot.gameObject);
            ingredientsItemSlots.Clear();

            foreach (ItemStack stack in recipe.Ingredients)
            {
                GameObject obj = Instantiate(phantomItemSlotPreFab, ingredientsContent.transform);
                if (!obj.TryGetComponent(out PhantomItemSlot slot)) continue;
                
                slot.ItemStack = stack;
                ingredientsItemSlots.Add(slot);
            }
        }
    }
}
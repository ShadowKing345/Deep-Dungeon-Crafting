using System.Collections.Generic;
using System.Linq;
using Inventory;
using Items;
using UnityEngine;

namespace Crafting
{
    public class CraftingManager : MonoBehaviour
    {
        public static CraftingManager instance;
        
        private readonly List<Recipe> _recipes = new List<Recipe>();

        private void Awake()
        {
            if (instance == null) instance = this;
            else if(instance != this) Destroy(gameObject);
            GetRecipes();
        }
        
        private void GetRecipes()
        {
            _recipes.Clear();
            _recipes.AddRange(Resources.LoadAll<Recipe>(""));
        }

        public bool CanCraft(IInventory inventory, Recipe recipe) => recipe.Ingredients.All(inventory.ContainsExact);

        public Recipe[] AvailableCrafts(IInventory inventory) => _recipes.Where(recipe => recipe.Ingredients.All(inventory.ContainsExact)).ToArray();

        public ItemStack[] GetMissingIngredients(IInventory inventory, Recipe recipe) => new ItemStack[0];

        public void Craft(IInventory inventory, Recipe recipe)
        {
            if (!CanCraft(inventory, recipe)) return;
            
            inventory.RemoveItemStacks(recipe.Ingredients);

            inventory.AddItemStacks(new[] {recipe.Result});
        }
        
        public Recipe[] Recipes => _recipes.ToArray();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Inventory;
using Items;
using UnityEngine;

namespace Crafting
{
    public class CraftingManager : MonoBehaviour
    {
        private static CraftingManager _instance;
        public static CraftingManager Instance
        {
            get
            {
                _instance ??= FindObjectOfType<CraftingManager>();
                return _instance;
            }
            private set
            {
                if (_instance != null && _instance != value)
                {
                    Destroy(value);
                    return;
                }
                _instance = value;
                DontDestroyOnLoad(value);
            }
        }
        
        private readonly List<Recipe> _recipes = new List<Recipe>();

        private void OnEnable()
        {
            Instance ??= this;
            Instance.GetRecipes();
        }

        private void GetRecipes()
        {
            _recipes.Clear();
            _recipes.AddRange(Resources.LoadAll<Recipe>("Recipes"));
        }

        public bool CanCraft(IInventory inventory, Recipe recipe) => recipe.Ingredients.All(inventory.ContainsExact);

        public Recipe[] AvailableCrafts(IInventory inventory) => _recipes.Where(recipe => recipe.Ingredients.All(inventory.ContainsExact)).ToArray();

        public Dictionary<ItemStack, bool> GetMissingIngredients(IInventory inventory, Recipe recipe) => recipe.Ingredients.ToDictionary(stack => stack, inventory.ContainsExact);

        public void Craft(IInventory inventory, Recipe recipe)
        {
            if (!CanCraft(inventory, recipe)) return;
            
            inventory.RemoveItemStacks(recipe.Ingredients);

            inventory.AddItemStacks(new[] {recipe.Result});
        }
        
        public Recipe[] Recipes => _recipes.ToArray();
    }
}
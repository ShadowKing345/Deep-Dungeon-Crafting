using System;
using System.Collections.Generic;
using System.Linq;
using Inventory;
using Items;
using UnityEditor;
using UnityEngine;

namespace Crafting
{
    public class CraftingManager
    {
        [SerializeField] private List<Recipe.Recipe> recipies = new List<Recipe.Recipe>();

        public void GetRecipes()
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(Recipe.Recipe)}");
            recipies.Clear();
            recipies.AddRange(guids.Select(id => AssetDatabase.LoadAssetAtPath<Recipe.Recipe>(AssetDatabase.GUIDToAssetPath(id))));
        }
        
        public bool CanCraft(IInventory inventory, Recipe.Recipe recipe) => recipe.ingredients.All(inventory.ContainsExact);

        public ItemStack[] GetMissingIngredients(IInventory inventory, Recipe.Recipe recipe) => new ItemStack[0];

        public void Craft(IInventory inventory, Recipe.Recipe recipe)
        {
            if (!CanCraft(inventory, recipe)) return;
            inventory.RemoveItemStacks(recipe.ingredients);
        }
    }
}
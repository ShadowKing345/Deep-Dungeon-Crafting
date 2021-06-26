using System;
using System.Collections.Generic;
using System.Linq;
using Inventory;
using Items;
using UnityEditor;
using UnityEngine;

namespace Crafting
{
    public class CraftingManager : MonoBehaviour
    {
        private readonly List<Recipe> _recipes = new List<Recipe>();

        private void Start()
        {
            GetRecipes();
        }

        private void GetRecipes()
        {
            _recipes.Clear();
            _recipes.AddRange(AssetDatabase.FindAssets($"t:{typeof(Recipe)}").Select(guid =>
                AssetDatabase.LoadAssetAtPath<Recipe>(AssetDatabase.GUIDToAssetPath(guid))));
        }

        public bool CanCraft(IInventory inventory, Recipe recipe) => recipe.ingredients.All(inventory.ContainsExact);

        public Recipe[] AvailableCrafts(IInventory inventory) => _recipes.Where(recipe => recipe.ingredients.All(inventory.ContainsExact)).ToArray();

        public ItemStack[] GetMissingIngredients(IInventory inventory, Recipe recipe) => new ItemStack[0];

        public void Craft(IInventory inventory, Recipe recipe)
        {
            if (!CanCraft(inventory, recipe)) return;
            
            inventory.RemoveItemStacks(recipe.ingredients);

            inventory.AddItemStacks(new[] {recipe.result});
        }
    }
}
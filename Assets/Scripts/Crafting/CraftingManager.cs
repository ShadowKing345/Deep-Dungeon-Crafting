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

        [SerializeField] private RecipeCollection recipeCollection;

        private void Awake() => Instance = this;

        public static bool CanCraft(IInventory inventory, Recipe recipe) => recipe.Ingredients.All(inventory.ContainsExact);

        public Recipe[] AvailableCrafts(IInventory inventory) => recipeCollection.recipes.Where(recipe => recipe.Ingredients.All(inventory.ContainsExact)).ToArray();

        public static Dictionary<ItemStack, bool> GetMissingIngredients(IInventory inventory, Recipe recipe) => recipe.Ingredients.ToDictionary(stack => stack, inventory.ContainsExact);

        public static void Craft(IInventory inventory, Recipe recipe)
        {
            if (!CanCraft(inventory, recipe)) return;
            
            inventory.RemoveItemStacks(recipe.Ingredients);

            inventory.AddItemStacks(new[] {recipe.Result});
        }
        
        public IEnumerable<Recipe> Recipes => recipeCollection.recipes.ToArray();
    }
}
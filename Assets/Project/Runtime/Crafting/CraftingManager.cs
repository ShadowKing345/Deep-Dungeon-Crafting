using System.Collections.Generic;
using System.Linq;
using Project.Runtime.Inventory;
using Project.Runtime.Items;
using UnityEngine;

namespace Project.Runtime.Crafting
{
    public class CraftingManager : MonoBehaviour
    {
        private static CraftingManager _instance;

        [SerializeField] private RecipeCollection recipeCollection;

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

        public IEnumerable<Recipe> Recipes => recipeCollection.recipes.ToArray();

        private void Awake()
        {
            Instance = this;
        }

        public static bool CanCraft(IInventory inventory, Recipe recipe)
        {
            return recipe.Ingredients.All(inventory.ContainsExact);
        }

        public Recipe[] AvailableCrafts(IInventory inventory)
        {
            return recipeCollection.recipes.Where(recipe => recipe.Ingredients.All(inventory.ContainsExact)).ToArray();
        }

        public static Dictionary<ItemStack, bool> GetMissingIngredients(IInventory inventory, Recipe recipe)
        {
            return recipe.Ingredients.ToDictionary(stack => stack, inventory.ContainsExact);
        }

        public static void Craft(IInventory inventory, Recipe recipe)
        {
            if (!CanCraft(inventory, recipe)) return;

            inventory.RemoveItemStacks(recipe.Ingredients);

            inventory.AddItemStacks(new[] {recipe.Result});
        }
    }
}
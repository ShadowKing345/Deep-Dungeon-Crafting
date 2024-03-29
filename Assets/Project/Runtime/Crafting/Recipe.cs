using System.Linq;
using Project.Runtime.Items;
using UnityEngine;

namespace Project.Runtime.Crafting
{
    [CreateAssetMenu(fileName = "New Recipe", menuName = "SO/Recipe/Recipe")]
    public class Recipe : ScriptableObject
    {
        [SerializeField] private ItemStack result;
        [SerializeField] private ItemStack[] ingredients;

        public ItemStack Result => result.Copy;
        public ItemStack[] Ingredients => ingredients.Select(ingredient => ingredient.Copy).ToArray();
    }
}
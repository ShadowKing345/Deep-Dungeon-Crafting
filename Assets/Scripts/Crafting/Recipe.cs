using System.Linq;
using Items;
using UnityEngine;

namespace Crafting
{
    [CreateAssetMenu(fileName = "New Recipe", menuName = "SO/Recipe")]
    public class Recipe : ScriptableObject
    {
        [SerializeField] private ItemStack result;
        [SerializeField] private ItemStack[] ingredients;

        public ItemStack Result => result.Copy;
        public ItemStack[] Ingredients => ingredients.Select(ingredient => ingredient.Copy).ToArray();
    }
}
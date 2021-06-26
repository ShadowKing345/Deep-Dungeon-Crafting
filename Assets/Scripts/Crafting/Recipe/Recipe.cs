using Items;
using UnityEngine;

namespace Crafting.Recipe
{
    [CreateAssetMenu(fileName = "New Recipe", menuName = "SO/Recipe")]
    public class Recipe : ScriptableObject
    {
        public ItemStack result;
        public ItemStack[] ingredients;
    }
}
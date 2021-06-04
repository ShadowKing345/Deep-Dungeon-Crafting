using UnityEngine;

namespace Items.Recipe
{
    public class Recipe : ScriptableObject
    {
        public ItemStack result;
        public ItemStack[] ingredients;
    }
}
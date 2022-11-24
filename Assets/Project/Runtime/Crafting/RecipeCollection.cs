using UnityEngine;

namespace Project.Runtime.Crafting
{
    [CreateAssetMenu(fileName = "New Recipe Collection", menuName = "SO/Recipe/Collection")]
    public class RecipeCollection : ScriptableObject
    {
        public Recipe[] recipes;
    }
}
using UnityEngine;

namespace Utils
{
    public static class GameObjectUtils
    {
        public static void ClearChildren(Transform transform)
        {
            foreach (Transform child in transform) Object.Destroy(child.gameObject);
        } 
    }
}
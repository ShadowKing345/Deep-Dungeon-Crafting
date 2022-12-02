using Project.Runtime.Utils.Interfaces;
using UnityEngine;

namespace Project.Runtime.Entity
{
    public class ChestEntity : MonoBehaviour, IInteractable
    {
        public bool Interact(GameObject target)
        {
            return true;
        }
    }
}
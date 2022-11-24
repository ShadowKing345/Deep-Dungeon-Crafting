using Project.Runtime.Enums;
using Project.Runtime.Managers;
using Project.Runtime.Utils.Interfaces;
using UnityEngine;

namespace Project.Runtime.Entity
{
    public class ChestEntity : MonoBehaviour, IInteractable
    {
        public bool Interact(GameObject target)
        {
            UiManager.Instance.ToggleUiElement(WindowReference.Chest);
            return true;
        }
    }
}
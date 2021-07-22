using Enums;
using Interfaces;
using Managers;
using UnityEngine;

namespace Entity
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
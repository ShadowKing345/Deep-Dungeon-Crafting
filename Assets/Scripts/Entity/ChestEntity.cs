using Enums;
using Managers;
using UnityEngine;
using Utils.Interfaces;

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
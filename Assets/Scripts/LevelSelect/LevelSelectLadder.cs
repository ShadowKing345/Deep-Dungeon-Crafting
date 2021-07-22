using Enums;
using Interfaces;
using Managers;
using UnityEngine;

namespace LevelSelect
{
    public class LevelSelectLadder : MonoBehaviour, IInteractable
    {
        public bool Interact(GameObject target)
        {
            UiManager.Instance.ToggleUiElement(WindowReference.LevelSelector);
            return true;
        }
    }
}
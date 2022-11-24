using Enums;
using Managers;
using UnityEngine;
using Utils.Interfaces;

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
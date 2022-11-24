using Project.Runtime.Enums;
using Project.Runtime.Managers;
using Project.Runtime.Utils.Interfaces;
using UnityEngine;

namespace Project.Runtime.LevelSelect
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
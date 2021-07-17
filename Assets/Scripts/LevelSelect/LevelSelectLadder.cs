using Interfaces;
using UnityEngine;

namespace LevelSelect
{
    public class LevelSelectLadder : MonoBehaviour, IInteractable
    {
        public bool Interact(GameObject target)
        {
            Debug.Log("Please Select Level");
            return true;
        }
    }
}
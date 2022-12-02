using Project.Runtime.Utils.Interfaces;
using UnityEngine;

namespace Project.Runtime.LevelSelect
{
    public class LevelSelectLadder : MonoBehaviour, IInteractable
    {
        public bool Interact(GameObject target)
        {
            return true;
        }
    }
}
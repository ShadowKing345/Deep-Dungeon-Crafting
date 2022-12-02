using Project.Runtime.Managers;
using Project.Runtime.Utils.Interfaces;
using UnityEngine;

namespace Project.Runtime.Level0
{
    public class ExitLevel0 : MonoBehaviour, IInteractable
    {
        public bool Interact(GameObject target)
        {
            return true;
        }
    }
}
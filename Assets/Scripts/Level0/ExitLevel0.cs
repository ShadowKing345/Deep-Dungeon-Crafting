using Managers;
using UnityEngine;
using Utils.Interfaces;

namespace Level0
{
    public class ExitLevel0 : MonoBehaviour, IInteractable
    {
        public bool Interact(GameObject target)
        {
            GameManager.Instance.LoadHub();
            return true;
        }
    }
}
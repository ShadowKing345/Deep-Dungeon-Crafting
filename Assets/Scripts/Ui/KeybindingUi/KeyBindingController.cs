using Settings;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Ui.KeybindingUi
{
    public class KeyBindingController : MonoBehaviour
    {
        [Header("Pre Fab")]
        public GameObject entryPreFab;
        [Space] 
        public Transform content;
        [Space]
        public KeyBindings[] entryCollection;
        
        public void GenerateKeyBindings()
        {
            foreach (KeyBindings entry in entryCollection)
            {
                foreach (InputActionReference reference in entry.actions)
                {
                    GameObject entryObj = Instantiate(entryPreFab, content.transform);
            
                    if (entryObj.TryGetComponent(out KeyBindingEntry keybindingEntry))
                        keybindingEntry.Action = reference;
                }
            }
        }
    }
}
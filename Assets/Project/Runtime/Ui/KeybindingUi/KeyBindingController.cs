using Settings;
using UnityEngine;

namespace Ui.KeybindingUi
{
    public class KeyBindingController : MonoBehaviour
    {
        [Header("Pre Fab")] public GameObject entryPreFab;

        [Space] public Transform content;

        [Space] public KeyBindings[] entryCollection;

        public void GenerateKeyBindings()
        {
            foreach (var entry in entryCollection)
            foreach (var reference in entry.actions)
            {
                var entryObj = Instantiate(entryPreFab, content.transform);

                if (entryObj.TryGetComponent(out KeyBindingEntry keybindingEntry))
                    keybindingEntry.Action = reference;
            }
        }
    }
}
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ui.KeybindingUi
{
    public class KeyBinding : MonoBehaviour
    {
        [Header("Pre Fab")]
        [SerializeField] private GameObject headerPreFab;
        [SerializeField] private GameObject entryPreFab;
        [Space] 
        [SerializeField] private GameObject content;

        [Serializable] private struct BindingCollection
        {
            public string header;
            public InputActionReference[] actions;
        }
        [Space] [SerializeField] private BindingCollection[] entryCollection;

        private void OnEnable()
        {
            headerPreFab.SetActive(false);
            entryPreFab.SetActive(false);
            
            GenerateKeyBindings();
        }
        
        private void GenerateKeyBindings()
        {
            foreach (Transform child in content.transform) Destroy(child.gameObject);

            foreach (BindingCollection entry in entryCollection)
            {
                GameObject headerObj = Instantiate(headerPreFab, content.transform);
                headerObj.SetActive(true);
                headerObj.GetComponentInChildren<TextMeshProUGUI>().text = entry.header;
                
                foreach (InputActionReference reference in entry.actions)
                {
                    GameObject entryObj = Instantiate(entryPreFab, content.transform);
                    entryObj.SetActive(true);
            
                    if (entryObj.TryGetComponent(out KeyBindingEntry keybindingEntry))
                        keybindingEntry.Action = reference;
                }
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{ 
        public class InputHandler : MonoBehaviour
        { 
                public static InputHandler instance;
                private readonly Dictionary<KeyValue, KeyCode> keys = new Dictionary<KeyValue, KeyCode>();

                private void Awake()
                {
                        if (instance == null)
                        {
                                instance = this;
                                DontDestroyOnLoad(gameObject);
                        }
                        else
                        {
                                Destroy(gameObject);
                        }

                        keys.Add(KeyValue.None, KeyCode.None);
                }

                private void Start() => ResetKeys();

                private void ResetKeys()
                {
                        keys.Clear();
                        keys.Add(KeyValue.OpenInventory, KeyCode.Tab);
                        keys.Add(KeyValue.OpenCraftingMenu, KeyCode.B);
                        keys.Add(KeyValue.PauseResumeGame, KeyCode.Escape);

                        keys.Add(KeyValue.Interact, KeyCode.Space);

                        keys.Add(KeyValue.UseHealthPotion, KeyCode.Q);
                        keys.Add(KeyValue.UseManaPotion, KeyCode.E);

                        keys.Add(KeyValue.ExecuteAction1, KeyCode.Alpha1);
                        keys.Add(KeyValue.ExecuteAction2, KeyCode.Alpha2);
                        keys.Add(KeyValue.ExecuteAction3, KeyCode.Alpha3);
                }

                public bool GetKeyDown(KeyValue keyValue) =>
                        keys.TryGetValue(keyValue, out KeyCode code) && Input.GetKeyDown(code);

                public bool GetKey(KeyValue keyValue) =>
                        keys.TryGetValue(keyValue, out KeyCode code) && Input.GetKey(code);

                public bool GetKeyUp(KeyValue keyValue) =>
                        keys.TryGetValue(keyValue, out KeyCode code) && Input.GetKeyUp(code);

                public KeyCode GetCodeFromValue(KeyValue keyValue) =>
                        keys.TryGetValue(keyValue, out KeyCode code) ? code : KeyCode.None;

                public void SaveKeys()
                {

                }

                public void LoadKeys()
                {

                }

                public enum KeyValue
                {
                        OpenInventory,
                        OpenCraftingMenu,
                        PauseResumeGame,

                        Interact,

                        UseHealthPotion,
                        UseManaPotion,

                        ExecuteAction1,
                        ExecuteAction2,
                        ExecuteAction3,

                        None
                }
        }
}
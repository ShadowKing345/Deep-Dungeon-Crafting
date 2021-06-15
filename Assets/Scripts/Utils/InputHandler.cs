using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
        public class InputHandler : MonoBehaviour
        {
                public static InputHandler instance;
                private readonly Dictionary<KeyValue, KeyCode> keys = new Dictionary<KeyValue, KeyCode>();

                private void OnEnable()
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
                }
                private void Start() => ResetKeys();

                private void ResetKeys()
                {
                        keys.Clear();
                        keys.Add(KeyValue.OpenInventory, KeyCode.Tab);
                        keys.Add(KeyValue.Interact, KeyCode.F);
                        keys.Add(KeyValue.UseHealthPotion, KeyCode.Q);
                        keys.Add(KeyValue.UseManaPotion, KeyCode.E);
                        keys.Add(KeyValue.ExecuteAction1, KeyCode.Alpha1);
                        keys.Add(KeyValue.ExecuteAction2, KeyCode.Alpha2);
                        keys.Add(KeyValue.ExecuteAction3, KeyCode.Alpha3);
                }

                public bool GetKeyDown(KeyValue keyValue) => keys.TryGetValue(keyValue, out KeyCode code) && Input.GetKeyDown(code);
                public bool GetKey(KeyValue keyValue) => keys.TryGetValue(keyValue, out KeyCode code) && Input.GetKey(code);
                public bool GetKeyUp(KeyValue keyValue) => keys.TryGetValue(keyValue, out KeyCode code) && Input.GetKeyUp(code);

                public void SaveKeys()
                {
                        
                }

                public void LoadKeys()
                {
                        
                }
                
                public enum KeyValue
                {
                        OpenInventory,
                        Interact,
                        UseHealthPotion,
                        UseManaPotion,
                        ExecuteAction1,
                        ExecuteAction2,
                        ExecuteAction3,
                }
        }
}
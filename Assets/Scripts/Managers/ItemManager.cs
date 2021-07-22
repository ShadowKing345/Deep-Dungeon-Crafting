using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using Newtonsoft.Json;
using UnityEngine;

namespace Managers
{
    public class ItemManager : MonoBehaviour, ISerializationCallbackReceiver
    {
        private static ItemManager _instance;

        public static ItemManager Instance
        {
            get
            {
                _instance ??= FindObjectOfType<ItemManager>();
                return _instance;
            }
            private set
            {
                if (_instance != null && _instance != value)
                {
                    Destroy(value);
                    return;
                }

                _instance = value;
                DontDestroyOnLoad(value);
            }
        }
        
        [SerializeField] private Item[] items;
        private readonly Dictionary<Item, int> itemId = new Dictionary<Item, int>();
        private readonly Dictionary<int, Item> idItem = new Dictionary<int, Item>();

        private void Awake() => Instance = this;

        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize() 
        {
            itemId.Clear();
            idItem.Clear();
            
            var i = 0;

            foreach (Item item in items)
            {
                itemId.Add(item, i);
                idItem.Add(i++, item);
            }
        }
        
        [Serializable]
        private class ConvertedItemStack
        {
            public int id = -1;
            public int amount;
        }
        
        public static string ItemStacksToJson(ItemStack[] stacks)
        {
           
            ConvertedItemStack[] convertedStacks = stacks.Select(stack => stack.IsEmpty ? new ConvertedItemStack() :
                new ConvertedItemStack
                {
                    id = Instance.itemId[stack.Item],
                    amount = stack.Amount
                }).ToArray();

            return JsonConvert.SerializeObject(convertedStacks);
        }

        public static ItemStack[] JsonToItemStacks(string json)
        {
            ConvertedItemStack[] convertedItemStacks = JsonConvert.DeserializeObject<ConvertedItemStack[]>(json);

            return convertedItemStacks
                .Select(stack => new ItemStack {Item = stack.id == -1 ? null : Instance.idItem[stack.id], Amount = stack.amount}).ToArray();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Project.Runtime.Entity.Player;
using Project.Runtime.Items;
using Project.Runtime.Systems;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Runtime.Ui.Inventories.ItemSlot.Utils
{
    [RequireComponent(typeof(ItemStackSlot))]
    public class ItemStackSlotContextMenu : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private ItemStackSlot slot;

        private void Awake()
        {
            slot ??= GetComponent<ItemStackSlot>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Right:
                    if (slot.ItemStack.IsEmpty) return;
                    var dictionary = new List<KeyValuePair<string, object>>
                    {
                        new("Destroy", new Action(Destroy))
                    };

                    if (slot.ItemStack.Item.MaxStackSize > 1)
                        dictionary.Insert(0, new KeyValuePair<string, object>("split", new Action(Split)));

                    switch (slot.ItemStack.Item)
                    {
                        case UsableItem _:
                            dictionary.InsertRange(0,
                                new[]
                                {
                                    new KeyValuePair<string, object>("Use", new Action(Use)),
                                    new KeyValuePair<string, object>("s", "")
                                });
                            break;
                        case ArmorItem _:
                        case WeaponItem _:
                            dictionary.InsertRange(0,
                                new[]
                                {
                                    new KeyValuePair<string, object>("Equip", new Action(Equip)),
                                    new KeyValuePair<string, object>("s", "")
                                });
                            break;
                    }

                    ContextMenuSystem.Instance.OpenContextMenu(dictionary.ToDictionary(k => k.Key, v => v.Value));
                    break;
            }
        }

        private void Use()
        {
            if (!(slot.ItemStack.Item is UsableItem usableItem)) return;
            if (!usableItem.Use(FindObjectOfType<PlayerEntity>())) return;

            slot.Inventory.GetStackAtSlot(slot.InventoryIndex).RemoveItem(1);
            slot.UpdateUi();
        }

        private void Split()
        {
            slot.Controller.SplitSlot(slot);
        }

        private void Destroy()
        {
            slot.Controller.ClearSlot(slot);
        }

        private void Equip()
        {
            Debug.Log("Equip");
        }
    }
}
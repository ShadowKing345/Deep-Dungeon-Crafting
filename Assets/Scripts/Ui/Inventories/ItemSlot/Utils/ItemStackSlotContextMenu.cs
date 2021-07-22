using System;
using System.Collections.Generic;
using System.Linq;
using Systems;
using Entity.Player;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ui.Inventories.ItemSlot.Utils
{
    [RequireComponent(typeof(ItemStackSlot))]
    public class ItemStackSlotContextMenu : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private ItemStackSlot slot;

        private void Awake() => slot ??= GetComponent<ItemStackSlot>();

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Right:
                    if (slot.ItemStack.IsEmpty) return;
                    List<KeyValuePair<string, object>> dictionary = new List<KeyValuePair<string, object>>()
                    {
                        new KeyValuePair<string, object>("Destroy", new Action(Destroy)),
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
            if (!usableItem.Use(FindObjectOfType<Player>())) return;
                
            slot.Inventory.GetStackAtSlot(slot.InventoryIndex).RemoveItem(1);
            slot.UpdateUi();
        }

        private void Split() => slot.Controller.SplitSlot(slot);

        private void Destroy() => slot.Controller.ClearSlot(slot);

        private void Equip() => Debug.Log("Equip");
    }
}
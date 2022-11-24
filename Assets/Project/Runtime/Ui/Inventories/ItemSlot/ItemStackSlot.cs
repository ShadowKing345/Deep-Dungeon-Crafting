using System;
using Inventory;
using Items;
using TMPro;
using Ui.Inventories.InventoryControllers;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Inventories.ItemSlot
{
    public class ItemStackSlot : MonoBehaviour, IItemStackSlot
    {
        [SerializeField] private ItemStack stack;

        [Space] [SerializeField] protected Image icon;

        [SerializeField] protected TextMeshProUGUI amount;


        private void Start()
        {
            UpdateUi();
        }

        public IInventoryController Controller { get; set; }

        public int Id { get; set; }

        public ItemStack ItemStack
        {
            get => stack;
            set
            {
                stack = value;
                UpdateUi();
            }
        }

        public int InventoryIndex { get; set; }
        public IInventory Inventory { get; set; }

        public void Init(int id, ItemStack stack, IInventoryController controller, int inventoryIndex,
            IInventory inventory)
        {
            Id = id;
            ItemStack = stack;
            Controller = controller;
            InventoryIndex = inventoryIndex;
            Inventory = inventory;
        }

        public virtual void UpdateUi()
        {
            if (ItemStack.IsEmpty)
            {
                icon.sprite = null;
                icon.color = Color.clear;
                amount.text = string.Empty;
            }
            else
            {
                icon.sprite = ItemStack.Item.Icon;
                icon.color = Color.white;
                amount.text = ItemStack.Amount > 1 ? ItemStack.Amount.ToString() : string.Empty;
            }
        }

        public void ResetSlot()
        {
            ItemStack = ItemStack.Empty;
        }

        public bool CanFit(ItemStack stack)
        {
            return Inventory.CanFitInSlot(stack, InventoryIndex);
        }
    }

    [Serializable]
    public struct SlotSprites
    {
        public Sprite normal;
        public Sprite mouseEnter;
        public Sprite active;
    }
}
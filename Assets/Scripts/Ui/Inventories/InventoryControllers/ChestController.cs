using System;
using System.Collections.Generic;
using Entity.Player;
using Enums;
using Inventory;
using Managers;
using Ui.Inventories.ItemSlot;
using UnityEngine;
using Utils;
using Utils.Interfaces;

namespace Ui.Inventories.InventoryControllers
{
    public class ChestController : MonoBehaviour, IInventoryController, IUiWindow
    {
        [SerializeField] private GameObject slotPreFab;

        [Space] [Header("Containers")] [SerializeField]
        private GameObject chestContainer;

        [SerializeField] private GameObject playerContainer;

        [Space] [SerializeField] private ItemInventory itemInventory;

        private readonly List<IItemStackSlot> _slots = new();

        private PlayerInventory _playerInventory;
        private UiManager _uiManager;
        private ItemInventory playerInventory;

        private void Awake()
        {
            _uiManager = UiManager.Instance;
            _uiManager.RegisterWindow(WindowReference.Chest, gameObject);
            gameObject.SetActive(false);

            var save = SaveManager.Instance.GetCurrentSave;
            if (save == null || string.IsNullOrEmpty(save.ChestInventory)) return;

            if (!ItemManager.TryJsonToItemStacks(save.ChestInventory, out var stacks)) return;

            itemInventory.ResetInventory();
            for (var i = 0; i < stacks.Length; i++) itemInventory.AddStackAtSlot(stacks[i], i);
        }

        private void OnEnable()
        {
            _playerInventory ??= FindObjectOfType<PlayerInventory>();

            if (_playerInventory == null) return;

            playerInventory = _playerInventory.ItemInventory;

            SetUpSlots();
        }

        private void OnDestroy()
        {
            _uiManager.UnregisterWindow(WindowReference.Chest, gameObject);

            if (!ItemManager.TryItemStacksToJson(itemInventory.GetItemStacks(), out var json)) return;
            SaveManager.Instance.GetCurrentSave.ChestInventory = json;
        }

        public IInventory GetInventory()
        {
            return itemInventory;
        }

        public void Init(IInventory inventory)
        {
            itemInventory = inventory as ItemInventory;
        }

        public void ExchangeItemStacks(IItemStackSlot from, IItemStackSlot to)
        {
            if (!to.CanFit(from.ItemStack)) return;

            if (from.Controller != to.Controller) CrossControllerExchange(from, to);
            else if (from.Inventory != to.Inventory) CrossInventoryExchange(from, to);
            else if (from.ItemStack.Item == to.ItemStack.Item)
                to.Inventory.CombineStacks(from.InventoryIndex, to.InventoryIndex);
            else to.Inventory.SwapSlots(from.InventoryIndex, to.InventoryIndex, out var _, out var _);

            from.UpdateUi();
            to.UpdateUi();
        }

        public void CrossInventoryExchange(IItemStackSlot from, IItemStackSlot to)
        {
            var fromInventory = from.Inventory;
            var toInventory = to.Inventory;

            var fromStack = fromInventory.RemoveStackAtSlot(from.InventoryIndex);

            if (to.ItemStack.IsEmpty)
            {
                toInventory.AddStackAtSlot(fromStack, to.InventoryIndex);
            }
            else
            {
                var toStack = toInventory.RemoveStackAtSlot(to.InventoryIndex);

                fromInventory.AddItemStacks(new[] {toInventory.AddStackAtSlot(fromStack, to.InventoryIndex)});
                toInventory.AddItemStacks(new[] {fromInventory.AddStackAtSlot(toStack, from.InventoryIndex)});
            }
        }

        public void CrossControllerExchange(IItemStackSlot from, IItemStackSlot to)
        {
            CrossInventoryExchange(from, to);
        }

        public void SplitSlot(IItemStackSlot slot)
        {
            slot.Inventory.SplitStack(slot.InventoryIndex, slot.ItemStack.Amount / 2);
            UpdateSlots();
        }

        public void ClearSlot(IItemStackSlot slot)
        {
            slot.Inventory.RemoveStackAtSlot(slot.InventoryIndex);
            slot.UpdateUi();
        }

        public void ResetSlots()
        {
            foreach (var slot in _slots)
                if (slot is MonoBehaviour mb)
                    Destroy(mb.gameObject);

            _slots.Clear();
        }

        public void UpdateSlots()
        {
            OnStackUpdate?.Invoke();
        }

        public void Show()
        {
            GameManager.PlayerMovement = false;
        }

        public void Hide()
        {
            GameManager.PlayerMovement = true;
        }

        private event Action OnStackUpdate;

        private void SetUpSlots()
        {
            var index = 0;
            _slots.Clear();
            GameObjectUtils.ClearChildren(chestContainer.transform);
            GameObjectUtils.ClearChildren(playerContainer.transform);

            if (slotPreFab == null) return;

            var itemIndex = 0;
            foreach (var stack in itemInventory.GetItemStacks())
            {
                var slotObj = Instantiate(slotPreFab, chestContainer.transform);
                if (!slotObj.TryGetComponent(out IItemStackSlot slot)) continue;

                slot.Init(index++, stack, this, itemIndex++, itemInventory);
                _slots.Add(slot);

                OnStackUpdate += slot.UpdateUi;
            }

            itemIndex = 0;
            foreach (var stack in playerInventory.GetItemStacks())
            {
                var slotObj = Instantiate(slotPreFab, playerContainer.transform);
                if (!slotObj.TryGetComponent(out IItemStackSlot slot)) continue;

                slot.Init(index++, stack, this, itemIndex++, playerInventory);
                _slots.Add(slot);

                OnStackUpdate += slot.UpdateUi;
            }
        }
    }
}
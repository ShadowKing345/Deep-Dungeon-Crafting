using System;
using System.Collections.Generic;
using System.IO;
using Entity.Player;
using Enums;
using Interfaces;
using Inventory;
using Items;
using Managers;
using Ui.Inventories.ItemSlot;
using UnityEngine;
using Utils;

namespace Ui.Inventories.InventoryControllers
{
    public class ChestController : MonoBehaviour, IInventoryController, IUiWindow
    {
        private UiManager _uiManager;
        private PlayerCombat playerCombat;
        private PlayerMovement playerMovement;

        [SerializeField] private GameObject slotPreFab;
        [Space] [Header("Containers")] 
        [SerializeField] private GameObject chestContainer;
        [SerializeField] private GameObject playerContainer;
        [Space]
        [SerializeField] private ItemInventory itemInventory;
        private ItemInventory playerInventory;
        
        private PlayerInventory _playerInventory;
        public IInventory GetInventory() => itemInventory;

        public void Init(IInventory inventory) => itemInventory = inventory as ItemInventory;

        private readonly List<IItemStackSlot> _slots = new List<IItemStackSlot>();
        
        private event Action OnStackUpdate;

        public void ExchangeItemStacks(IItemStackSlot @from, IItemStackSlot to)
        {
            if (!to.CanFit(from.ItemStack)) return;

            if (from.Controller != to.Controller) CrossControllerExchange(from, to);
            else if(from.Inventory != to.Inventory) CrossInventoryExchange(from, to);
            else if (from.ItemStack.Item == to.ItemStack.Item)
                to.Inventory.CombineStacks(from.InventoryIndex, to.InventoryIndex);
            else to.Inventory.SwapSlots(from.InventoryIndex, to.InventoryIndex, out var _, out var _);
            
            from.UpdateUi();
            to.UpdateUi();
        }

        public void CrossInventoryExchange(IItemStackSlot @from, IItemStackSlot to)
        {
            IInventory fromInventory = from.Inventory;
            IInventory toInventory = to.Inventory;
            
            ItemStack fromStack = fromInventory.RemoveStackAtSlot(from.InventoryIndex);

            if (to.ItemStack.IsEmpty)
                toInventory.AddStackAtSlot(fromStack, to.InventoryIndex);
            else
            {
                ItemStack toStack = toInventory.RemoveStackAtSlot(to.InventoryIndex);

                fromInventory.AddItemStacks(new[] {toInventory.AddStackAtSlot(fromStack, to.InventoryIndex)});
                toInventory.AddItemStacks(new[] {fromInventory.AddStackAtSlot(toStack, from.InventoryIndex)});
            }
        }

        public void CrossControllerExchange(IItemStackSlot @from, IItemStackSlot @to) => CrossInventoryExchange(@from, to);
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
            foreach (IItemStackSlot slot in _slots)
                if(slot is MonoBehaviour mb)
                    Destroy( mb.gameObject);

            _slots.Clear();
        }

        public void UpdateSlots() => OnStackUpdate?.Invoke();

        private void Awake()
        {
            _uiManager = UiManager.Instance;
            _uiManager.RegisterWindow(WindowReference.Chest, gameObject);
            gameObject.SetActive(false);

            if (!SaveSystem.TryLoadObj(Path.Combine(GameManager.Instance.savePath, "chest.inv"),
                out string json)) return;

            itemInventory.ResetInventory();
            itemInventory.AddItemStacks(ItemManager.JsonToItemStacks(json));
        }

        private void OnDestroy()
        {
            _uiManager.UnregisterWindow(WindowReference.Chest,gameObject);
            
            SaveSystem.SaveObj(Path.Combine(GameManager.Instance.savePath, "chest.inv"),
                ItemManager.ItemStacksToJson(itemInventory.GetItemStacks()));
        }

        private void OnEnable()
        {
            playerCombat ??= FindObjectOfType<PlayerCombat>();
            playerMovement ??= FindObjectOfType<PlayerMovement>();
            
            _playerInventory ??= FindObjectOfType<PlayerInventory>();

            if (_playerInventory == null) return;

            playerInventory = _playerInventory.ItemInventory;
            
            SetUpSlots();
        }

        private void SetUpSlots()
        {
            int index = 0;
            _slots.Clear();
            GameObjectUtils.ClearChildren(chestContainer.transform);
            GameObjectUtils.ClearChildren(playerContainer.transform);

            if(slotPreFab == null) return;

            int itemIndex = 0;
            foreach (ItemStack stack in itemInventory.GetItemStacks())
            {
                GameObject slotObj = Instantiate(slotPreFab, chestContainer.transform);
                if(!slotObj.TryGetComponent(out IItemStackSlot slot)) continue;
                
                slot.Init(index++, stack, this, itemIndex++, itemInventory);
                _slots.Add(slot);

                OnStackUpdate += slot.UpdateUi;
            }

            itemIndex = 0;
            foreach (ItemStack stack in playerInventory.GetItemStacks())
            {
                GameObject slotObj = Instantiate(slotPreFab, playerContainer.transform);
                if(!slotObj.TryGetComponent(out IItemStackSlot slot)) continue;
                
                slot.Init(index++, stack, this, itemIndex++, playerInventory);
                _slots.Add(slot);

                OnStackUpdate += slot.UpdateUi;
            }
        }

        public void Show() => playerCombat.enabled = playerMovement.enabled = false;
        public void Hide() => playerCombat.enabled = playerMovement.enabled = true;
    }
}
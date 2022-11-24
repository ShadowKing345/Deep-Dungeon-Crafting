using System;
using System.Collections.Generic;
using System.Linq;
using Project.Runtime.Entity.Player;
using Project.Runtime.Enums;
using Project.Runtime.Inventory;
using Project.Runtime.Managers;
using Project.Runtime.Ui.Inventories.ItemSlot;
using Project.Runtime.Utils;
using Project.Runtime.Utils.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Project.Runtime.Ui.Inventories.InventoryControllers
{
    public class PlayerInventoryController : MonoBehaviour, IInventoryController, IUiWindow
    {
        [FormerlySerializedAs("playerMovementController")] [SerializeField]
        private PlayerMovement playerMovementController;

        [SerializeField] private PlayerCombat playerCombatController;

        [Header("Item Inventory")] [SerializeField]
        private GameObject slotPrefab;

        [SerializeField] private GameObject container;

        [Space] [Header("Weapon Inventory")] [SerializeField]
        private ItemStackSlot weaponItemStackSlot;

        [Space] [Header("Armor Inventory")] [SerializeField]
        private ItemStackSlot[] armorItemStackSlots = new ItemStackSlot[6];

        [Space] [SerializeField] private PlayerInventory playerInventory;

        private readonly List<IItemStackSlot> _slots = new();
        private ArmorInventory _armorInventory;

        private ItemInventory _itemInventory;
        private UiManager _uiManager;
        private WeaponInventory _weaponInventory;

        private void Awake()
        {
            _uiManager = UiManager.Instance;
            _uiManager.RegisterWindow(WindowReference.PlayerInventory, gameObject);

            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            playerMovementController ??= FindObjectOfType<PlayerMovement>();
            playerCombatController ??= FindObjectOfType<PlayerCombat>();

            playerInventory ??= FindObjectOfType<PlayerInventory>();
            if (playerInventory == null) return;

            _weaponInventory = playerInventory.WeaponInventory;
            _armorInventory = playerInventory.ArmorInventory;
            _itemInventory = playerInventory.ItemInventory;

            SetUpSlots();
            container.GetComponentInChildren<Selectable>().Select();
        }

        private void OnDestroy()
        {
            _uiManager.UnregisterWindow(WindowReference.PlayerInventory, gameObject);
        }

        public IInventory GetInventory()
        {
            return _itemInventory;
        }

        public void Init(IInventory inventory)
        {
            _itemInventory = (ItemInventory) inventory;
            SetUpSlots();
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
            foreach (var slot in _slots.Where(slot =>
                         (ItemStackSlot) slot != weaponItemStackSlot && !armorItemStackSlots.Contains(slot)))
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
            playerMovementController.enabled = playerCombatController.enabled = false;
        }

        public void Hide()
        {
            playerMovementController.enabled = playerCombatController.enabled = true;
        }

        private event Action OnStackUpdate;

        public void SetUpSlots()
        {
            GameObjectUtils.ClearChildren(container.transform);
            _slots.Clear();
            OnStackUpdate = null;

            var index = 0;

            //WeaponSlotSetup
            if (weaponItemStackSlot != null)
            {
                weaponItemStackSlot.Init(index++, _weaponInventory.GetStackAtSlot(0), this, 0, _weaponInventory);
                _slots.Add(weaponItemStackSlot);
            }

            var armorIndex = 0;
            //ArmorSlotSetup
            foreach (IItemStackSlot slot in armorItemStackSlots)
            {
                if (slot == null) continue;

                slot.Init(index++, _armorInventory.GetStackAtSlot(armorIndex), this, armorIndex++, _armorInventory);
                _slots.Add(slot);
            }

            //ItemInventorySetup
            if (slotPrefab == null) return;
            var itemIndex = 0;
            foreach (var stack in _itemInventory.GetItemStacks())
            {
                var slotObj = Instantiate(slotPrefab, container.transform);
                if (!slotObj.TryGetComponent(out IItemStackSlot slot)) continue;

                slot.Init(index++, stack, this, itemIndex++, _itemInventory);
                _slots.Add(slot);

                OnStackUpdate += slot.UpdateUi;
            }

            UpdateSlots();
        }
    }
}
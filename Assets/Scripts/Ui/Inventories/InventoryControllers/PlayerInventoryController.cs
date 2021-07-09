using System;
using System.Collections.Generic;
using System.Linq;
using Entity.Player;
using Inventory;
using Items;
using Ui.Inventories.ItemSlot;
using UnityEngine;

namespace Ui.Inventories.InventoryControllers
{
    public class PlayerInventoryController : MonoBehaviour, IInventoryController
    {
        private static PlayerInventoryController _instance;
        public static PlayerInventoryController Instance {
            get
            {
                if (_instance != null) return _instance;

                _instance = FindObjectOfType<PlayerInventoryController>();
                return _instance;
            }
            private set
            {
                if (_instance != null && _instance != value)
                {
                    Destroy(value.gameObject);
                    return;
                }

                _instance = value;
            }
        }
        
        [Header("Item Inventory")]
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private GameObject container;
        [Space]
        [Header("Weapon Inventory")] 
        [SerializeField] private ItemStackSlot weaponItemStackSlot;
        [Space]
        [Header("Armor Inventory")] 
        [SerializeField] private ItemStackSlot[] armorItemStackSlots = new ItemStackSlot[6];
        [Space]
        [SerializeField] private PlayerInventory playerInventory;

        private ItemInventory _itemInventory;
        private WeaponInventory _weaponInventory;
        private ArmorInventory _armorInventory;

        public IInventory GetInventory() => _itemInventory;
        private readonly List<IItemStackSlot> _slots = new List<IItemStackSlot>();
        
        private event Action OnStackUpdate; 

        private void OnEnable()
        {
            Instance ??= this;
            playerInventory ??= FindObjectOfType<PlayerInventory>();
            if (playerInventory == null) return;
            
            _weaponInventory = playerInventory.WeaponInventory;
            _armorInventory = playerInventory.ArmorInventory;
            _itemInventory = playerInventory.ItemInventory;
            
            SetUpSlots();
        }

        public void Init(IInventory inventory)
        {
            _itemInventory = (ItemInventory) inventory;
            SetUpSlots();
        }
        
        public void SetUpSlots()
        {
            int index = 0;
            _slots.Clear();
            foreach (Transform child in container.transform) Destroy(child.gameObject);

            //WeaponSlotSetup
            if (weaponItemStackSlot != null)
            {
                weaponItemStackSlot.Init(index++, _weaponInventory.GetStackAtSlot(0), this, 0, _weaponInventory);
                _slots.Add(weaponItemStackSlot);
            }

            int armorIndex = 0;
            //ArmorSlotSetup
            foreach (IItemStackSlot slot in armorItemStackSlots)
            {
                if (slot == null) continue;
                
                slot.Init(index++, _armorInventory.GetStackAtSlot(armorIndex), this, armorIndex++, _armorInventory);
                _slots.Add(slot);
            }

            //ItemInventorySetup
            if (slotPrefab == null) return;
            int itemIndex = 0;
            foreach(ItemStack stack in _itemInventory.GetItemStacks())
            {
                GameObject slotObj = Instantiate(slotPrefab, container.transform);
                if (!slotObj.TryGetComponent(out IItemStackSlot slot)) continue;
                
                slot.Init(index++, stack, this, itemIndex++, _itemInventory);
                _slots.Add(slot);
                
                OnStackUpdate += slot.UpdateUi;
            }
            
            UpdateSlots();
        }

        public void ExchangeItemStacks(IItemStackSlot @from, IItemStackSlot @to)
        {
            if (!to.CanFit(from.ItemStack)) return;

            if (from.Controller != to.Controller) CrossControllerExchange(from, to);
            else if(from.Inventory != to.Inventory) CrossInventoryExchange(from, to);
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

        public void ResetSlots()
        {
            foreach (var slot in _slots.Where(slot => (ItemStackSlot) slot != weaponItemStackSlot && !armorItemStackSlots.Contains(slot)))
                if(slot is MonoBehaviour mb)
                    Destroy( mb.gameObject);

            _slots.Clear();
        }

        public void UpdateSlots() => OnStackUpdate?.Invoke();
    }
}
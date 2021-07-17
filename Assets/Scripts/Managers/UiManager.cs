using System;
using System.Collections.Generic;
using Combat;
using Entity.Player;
using Interfaces;
using Items;
using Ui.HudElements;
using Ui.Inventories;
using UnityEngine;
using ProgressBar = Ui.ProgressBar;

namespace Managers
{
    public class UiManager : MonoBehaviour
    {
        private static UiManager _instance;
            public static UiManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = FindObjectOfType<UiManager>();
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

        public Canvas canvas;
        private InputManager inputManager;

        [Serializable]
        private struct UiElements
        {
            public ProgressBar healthProgressBar;
            public ProgressBar manaProgressBar;

            public AbilityUi abilityUi1;
            public AbilityUi abilityUi2;
            public AbilityUi abilityUi3;
        }

        [Serializable]
        private struct WindowGameObjs
        {
            public GameObject pauseMenu;
            public GameObject inGameHelp;
            public GameObject craftingMenu;
            public GameObject playerMenu;
        }

        [SerializeField] private UiElements hudElements;
        [Space] [SerializeField] private GameObject itemHovePreFab;
        [SerializeField] private GameObject itemHoverObj;
        [Space] [SerializeField] private WindowGameObjs windowGameObjs;
        [SerializeField] private GameObject hudObj;
        [Space] [SerializeField] private UiElementReference currentActive;
        
        private void OnEnable()
        {
            Instance = this;
            
            inputManager ??= new InputManager();

            InputManager.WindowsActions windowsActions = inputManager.Windows;
            
            windowsActions.PauseMenu.canceled += _ => ToggleUiElement(UiElementReference.PauseMenu);
            windowsActions.Crafting.canceled += _ => ToggleUiElement(UiElementReference.CraftingMenu);
            windowsActions.PlayerInventory.canceled += _ => ToggleUiElement(UiElementReference.PlayerInventory);

            windowsActions.Enable();
        }

        private void OnDisable() => inputManager.Windows.Disable();

        public void BeginItemHover(ItemStack stack)
        {
            if (itemHoverObj != null) EndItemHover();

            itemHoverObj = Instantiate(itemHovePreFab, canvas.transform);
            itemHoverObj.GetComponent<HoverItem>().Init(stack);
        }

        public void EndItemHover()
        {
            if (itemHoverObj == null) return;

            Destroy(itemHoverObj);
            itemHoverObj = null;
        }

        #region Hud

        public void SetMaxHealthMana(float health, float mana)
        {
            if (hudElements.healthProgressBar != null) hudElements.healthProgressBar.MAX = health;
            if (hudElements.manaProgressBar != null) hudElements.manaProgressBar.MAX = mana;
        }

        public void SetHealthMana(float health, float mana)
        {
            if (hudElements.healthProgressBar != null) hudElements.healthProgressBar.Current = health;
            if (hudElements.manaProgressBar != null) hudElements.manaProgressBar.Current = mana;
        }

        public void InitializeAbilityUi(PlayerCombat combatController, Dictionary<WeaponClass.AbilityIndex, Ability[]> dictionary)
        {
            foreach (KeyValuePair<WeaponClass.AbilityIndex,Ability[]> kvPair in dictionary)
                if (TryGetAbility(kvPair.Key, out AbilityUi abilityUi))
                    abilityUi.SetUp(combatController, kvPair.Key, kvPair.Value);
        }

        public void SetAbilityUiComboIndex(WeaponClass.AbilityIndex index, int comboIndex)
        {
            if(TryGetAbility(index, out AbilityUi ui)) ui.SetAbility(comboIndex);
        }

        public void SetAbilityUiCoolDown(WeaponClass.AbilityIndex index, float amount)
        {
            if(TryGetAbility(index, out AbilityUi ui)) ui.SetCoolDown(amount);
        }
        
        private bool TryGetAbility(WeaponClass.AbilityIndex index, out AbilityUi abilityUi)
        {
            abilityUi = index switch
            {
                WeaponClass.AbilityIndex.Abilities1 => hudElements.abilityUi1,
                WeaponClass.AbilityIndex.Abilities2 => hudElements.abilityUi2,
                WeaponClass.AbilityIndex.Abilities3 => hudElements.abilityUi3,
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
            return abilityUi != null;
        }

        #endregion

        #region Window Management

        public void ToggleUiElement(UiElementReference element)
        {
            if (element == UiElementReference.PauseMenu && currentActive != UiElementReference.PauseMenu &&
                currentActive != UiElementReference.None)
            {
                HideUiElement(currentActive);
                return;
            }

            if (IsUiElementShown(element)) HideUiElement(element);
            else ShowUiElement(element);
        }

        private void ShowUiElement(UiElementReference element)
        {
            if (currentActive != UiElementReference.None) return;
            
            GameObject windowObj = GetGameObject(element);
            if(windowObj == null) return;
            
            windowObj.SetActive(true);
            currentActive = element;
            
            if (windowObj.TryGetComponent(out IUiWindow uiWindow))
                uiWindow.Show();
        }

        public void HideUiElement(UiElementReference element)
        {
            if (currentActive == UiElementReference.None || currentActive != element) return;
            
            GameObject windowObj = GetGameObject(element);
            if(windowObj == null) return;
            
            windowObj.SetActive(false);
            currentActive = UiElementReference.None;
            
            if (windowObj.TryGetComponent(out IUiWindow uiWindow))
                uiWindow.Hide();
        }

        private bool IsUiElementShown(UiElementReference element) => GetGameObject(element).activeSelf;

        private GameObject GetGameObject(UiElementReference element) => element switch
        {
            UiElementReference.None => null,
            UiElementReference.PauseMenu => windowGameObjs.pauseMenu,
            UiElementReference.Help => windowGameObjs.inGameHelp,
            UiElementReference.CraftingMenu => windowGameObjs.craftingMenu,
            UiElementReference.PlayerInventory => windowGameObjs.playerMenu,
            _ => throw new ArgumentOutOfRangeException(nameof(element), element, null)
        };

        #endregion
        
        public enum UiElementReference
        {
            None,
            PauseMenu,
            Help,
            CraftingMenu,
            PlayerInventory
        }
    }
}
using System;
using System.Collections.Generic;
using Combat;
using Entity.Player;
using Interfaces;
using Items;
using Ui.HudElements;
using Ui.Inventories;
using Ui.ToolTip;
using Ui.ToolTip.Types;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Utils;
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
        private struct WindowGameObj
        {
            public GameObject pauseMenuCg;
            public GameObject inGameHelpCg;
            public GameObject craftingMenuCg;
            public GameObject playerMenuCg;
            public GameObject hud;
        }

        [SerializeField] private UiElements hudElements;
        [Space] [SerializeField] private GameObject itemHovePreFab;
        [SerializeField] private GameObject itemHoverObj;
        [Space] [SerializeField] private WindowGameObj windowGameObj;
        [Space] [SerializeField] private UiElementReference currentActive;
        
        private void OnEnable()
        {
            Instance ??= this;
            
            inputManager ??= new InputManager();

            inputManager.Windows.PlayerInventory.canceled += _ => ToggleUiElement(UiElementReference.PlayerMenu);
            inputManager.Windows.Crafting.canceled += _ => ToggleUiElement(UiElementReference.CraftingMenu);
            inputManager.Windows.PauseMenu.canceled += _ =>
            {
                if (currentActive == UiElementReference.PauseMenu || currentActive == UiElementReference.None)
                    ToggleUiElement(UiElementReference.PauseMenu);
                else
                    HideUiElement(currentActive);
            };

            inputManager.Windows.Enable();
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
            if (IsUiElementShown(element)) HideUiElement(element);
            else ShowUiElement(element);
        }

        private void ShowUiElement(UiElementReference element)
        {
            if (currentActive != element && currentActive != UiElementReference.None) HideUiElement(currentActive);

            currentActive = element;
            GameObject obj = GetGameObject(element);
            obj.SetActive(true);

            if (!obj.TryGetComponent(out CanvasGroup cg)) return;
            
            LeanTween.alphaCanvas(cg, 1, .1f).setIgnoreTimeScale(true).setOnComplete(_ =>
            {
                if (cg.TryGetComponent(out IUiElement uiElement)) uiElement.Show();
            });
        }

        public void HideUiElement(UiElementReference element)
        {
            GameObject obj = GetGameObject(element);
            if (obj.TryGetComponent(out CanvasGroup cg))
            {
                LeanTween.alphaCanvas(cg, 0, .1f).setIgnoreTimeScale(true).setOnComplete(_ =>
                {
                    if (obj.TryGetComponent(out IUiElement uiElement)) uiElement.Hide();
                    obj.SetActive(false);
                });
            }
            else 
                obj.SetActive(false);
            
            currentActive = UiElementReference.None;
        }

        private bool IsUiElementShown(UiElementReference element) => GetGameObject(element).activeSelf;

        private GameObject GetGameObject(UiElementReference element) => element switch
        {
            UiElementReference.PauseMenu => windowGameObj.pauseMenuCg,
            UiElementReference.Help => windowGameObj.inGameHelpCg,
            UiElementReference.CraftingMenu => windowGameObj.craftingMenuCg,
            UiElementReference.PlayerMenu => windowGameObj.playerMenuCg,
            UiElementReference.Hud => windowGameObj.hud,
            _ => throw new ArgumentOutOfRangeException(nameof(element), element, null)
        };

        #endregion
        
        public enum UiElementReference
        {
            PauseMenu,
            Help,
            CraftingMenu,
            PlayerMenu,
            Hud,
            None
        }
    }
}
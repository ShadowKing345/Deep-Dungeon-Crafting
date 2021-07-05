using System;
using Combat;
using Interfaces;
using Items;
using Ui;
using Ui.HudElements;
using Ui.Inventories;
using Ui.ToolTip;
using Ui.ToolTip.Types;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Managers
{
    public class WindowManager : MonoBehaviour
    {
        public static WindowManager instance;

        public Canvas canvas;

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
        private struct CanvasGroups
        {
            public CanvasGroup pauseMenuCg;
            public CanvasGroup inGameHelpCg;
            public CanvasGroup craftingMenuCg;
            public CanvasGroup playerMenuCg;
            public CanvasGroup hud;
        }

        [SerializeField] private UiElements hudElements;
        [Space] [SerializeField] private GameObject itemHovePreFab;
        [SerializeField] private GameObject itemHoverObj;
        [Space] [SerializeField] private CanvasGroups canvasGroups;
        [Space] [SerializeField] private UiElementReference currentActive;

        private void Awake()
        {
            if (instance == null) instance = this;
            else if (instance != this) Destroy(gameObject);
        }

        private void Start()
        {
            foreach (WeaponClass.AbilityIndex index in Enum.GetValues(typeof(WeaponClass.AbilityIndex)))
                if (TryGetAbility(index, out var abilityUi))
                    abilityUi.AbilityIndex = index;
        }

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

        public void SetAbility(WeaponClass.AbilityIndex index, Ability ability, bool comboActive = false)
        {
            if (!TryGetAbility(index, out var abilityUi)) return;

            abilityUi.Ability = ability;
            if (abilityUi is IHudElement a) a.UpdateUi();
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
            CanvasGroup cg = GetCanvasGroup(element);

            LeanTween.alphaCanvas(cg, 1, .1f).setIgnoreTimeScale(true).setOnComplete(_ =>
            {
                cg.interactable = true;
                cg.blocksRaycasts = true;
                if (cg.TryGetComponent(out IUiElement uiElement)) uiElement.Show();
            });
        }

        public void HideUiElement(UiElementReference element)
        {
            CanvasGroup cg = GetCanvasGroup(element);
            LeanTween.alphaCanvas(cg, 0, .1f).setIgnoreTimeScale(true).setOnComplete(_ =>
            {
                cg.interactable = false;
                cg.blocksRaycasts = false;
                if (cg.TryGetComponent(out IUiElement uiElement)) uiElement.Hide();
            });
            currentActive = UiElementReference.None;
        }

        private bool IsUiElementShown(UiElementReference element) => GetCanvasGroup(element).interactable;

        private CanvasGroup GetCanvasGroup(UiElementReference element) => element switch
        {
            UiElementReference.PauseMenu => canvasGroups.pauseMenuCg,
            UiElementReference.Help => canvasGroups.inGameHelpCg,
            UiElementReference.CraftingMenu => canvasGroups.craftingMenuCg,
            UiElementReference.PlayerMenu => canvasGroups.playerMenuCg,
            UiElementReference.Hud => canvasGroups.hud,
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
        
        public void OpenInventory(InputAction.CallbackContext ctx)
        {
            if(ctx.canceled) ToggleUiElement(WindowManager.UiElementReference.PlayerMenu);
        }

        public void OpenCraftingMenu(InputAction.CallbackContext ctx)
        {
            if(ctx.canceled)
                ToggleUiElement(WindowManager.UiElementReference.CraftingMenu);
        }

        public void OpenPauseMenu(InputAction.CallbackContext ctx)
        {
            if(ctx.canceled) ToggleUiElement(WindowManager.UiElementReference.PauseMenu);
        }
    }
}
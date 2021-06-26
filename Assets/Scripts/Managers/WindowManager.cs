using System;
using Combat;
using Interfaces;
using Items;
using Ui;
using Ui.HudElements;
using Ui.Inventories;
using UnityEngine;
using Utils;

namespace Managers
{
    public class WindowManager : MonoBehaviour
    {
        public static WindowManager instance;

        public Canvas canvas;
        
        [Serializable] private struct UiElements
        {
            public ProgressBar healthProgressBar;
            public ProgressBar manaProgressBar;

            public AbilityUi abilityUi1;
            public AbilityUi abilityUi2;
            public AbilityUi abilityUi3;
        }
        [SerializeField] private UiElements hudElements;
        
        [SerializeField] private GameObject itemHoverObj;

        [Serializable] private struct CanvasGroups
        {
            public CanvasGroup pauseMenuCG;
            public CanvasGroup journalUiCG;
            public CanvasGroup craftingMenuCG;
            public CanvasGroup playerMenuCG;
            public CanvasGroup hud;
        }

        [SerializeField] private CanvasGroups canvasGroups;

        private void Awake()
        {
            if (instance == null) instance = this; else if (instance != this) Destroy(gameObject);
        }

        private void Start()
        {
            foreach (WeaponClass.AbilityIndex index in Enum.GetValues(typeof(WeaponClass.AbilityIndex)))
                if (TryGetAbility(index, out var abilityUi))
                    abilityUi.AbilityIndex = index;
            
            SetAbilityKeyCode(WeaponClass.AbilityIndex.Abilities1, InputHandler.KeyValue.ExecuteAction1);
            SetAbilityKeyCode(WeaponClass.AbilityIndex.Abilities2, InputHandler.KeyValue.ExecuteAction2);
            SetAbilityKeyCode(WeaponClass.AbilityIndex.Abilities3, InputHandler.KeyValue.ExecuteAction3);
        }

        public void BeginItemHover(GameObject preFab, ItemStack stack)
        {
            if (itemHoverObj != null) EndItemHover();
            
            itemHoverObj = Instantiate(preFab, canvas.transform);
            itemHoverObj.GetComponent<HoverItem>().Init(stack, canvas);
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
            if(abilityUi is IHudElement a) a.UpdateUi();
        }

        public void SetAbilityKeyCode(WeaponClass.AbilityIndex index, InputHandler.KeyValue keyValue)
        {
            if (!TryGetAbility(index, out var abilityUi)) return;

            abilityUi.KeyValue = keyValue;
            if(abilityUi is IHudElement a) a.UpdateUi();
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

        public void ShowUiElement(UiElementReference element)
        {
            CanvasGroup cg = GetCanvasGroup(element);

            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
            if(cg.TryGetComponent(out IUiElement uiElement)) uiElement.Show();
        }
        public void HideUiElement(UiElementReference element)
        {
            CanvasGroup cg = GetCanvasGroup(element);

            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
            if(cg.TryGetComponent(out IUiElement uiElement)) uiElement.Hide();
        }

        public bool IsUiElementShown(UiElementReference element) => GetCanvasGroup(element).interactable;
        
        public CanvasGroup GetCanvasGroup(UiElementReference element) => element switch
        {
            UiElementReference.PauseMenu => canvasGroups.pauseMenuCG,
            UiElementReference.Journal => canvasGroups.journalUiCG,
            UiElementReference.CraftingMenu => canvasGroups.craftingMenuCG,
            UiElementReference.PlayerMenu => canvasGroups.playerMenuCG,
            UiElementReference.Hud => canvasGroups.hud,
            _ => throw new ArgumentOutOfRangeException(nameof(element), element, null)
        };
        
        #endregion
        
        public enum UiElementReference
        {
            PauseMenu,
            Journal,
            CraftingMenu,
            PlayerMenu,
            Hud
        }
    }
}
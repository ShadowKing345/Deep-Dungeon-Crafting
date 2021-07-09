using System;
using Combat;
using Entity.Player;
using Managers;
using TMPro;
using Ui.ToolTip;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Ui.HudElements
{
    [ExecuteInEditMode]
    public class AbilityUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private static InputManager _inputManager;
        private LTDescr progressBarId;

        [Header("Components")] [SerializeField]
        private Image image;

        [SerializeField] private TextMeshProUGUI keybindingText;
        [SerializeField] private GameObject comboIndicator;
        [SerializeField] private ProgressBar coolDownProgressBar;

        [Space] [Header("Private Variables")] [SerializeField]
        private PlayerCombat playerCombat;

        [SerializeField] private WeaponClass.AbilityIndex index;
        [SerializeField] private Ability[] abilities;
        [SerializeField] private int currentAbility;

        [Serializable]
        private struct InputActionReferences
        {
            public InputActionReference ability1;
            public InputActionReference ability2;
            public InputActionReference ability3;
        }
        [Space]
        [SerializeField] private InputActionReferences inputActionReferences; 

        public void SetUp(PlayerCombat combatController, WeaponClass.AbilityIndex abilityIndex, Ability[] abilities)
        {
            playerCombat = combatController;
            index = abilityIndex;
            this.abilities = abilities;

            SetAbility(0);
        }

        private void OnEnable() => _inputManager ??= new InputManager();

        public void SetAbility(int comboIndex)
        {
            currentAbility = comboIndex;
            comboIndicator.SetActive(comboIndex > 0);
            if (comboIndex >= abilities.Length)
            {
                ClearUi();
                return;
            }

            if (abilities.Length <= 0)
            {
                ClearUi();
                return;
            }

            Ability ability = abilities[comboIndex];
            if (ability == null)
            {
                ClearUi();
                return;
            }

            image.sprite = ability.Icon;
            image.color = Color.white;
            
            keybindingText.text = GetKeyBindingText();
        }

        public void SetCoolDown(float amount) => progressBarId = LeanTween.value(gameObject, 100, 0, amount)
            .setOnUpdate(value => coolDownProgressBar.Current = value);

        public void Attack()
        {
            if (playerCombat == null || coolDownProgressBar.Current > 0) return;
            playerCombat.ExecuteAbility(index);
        }

        private void ClearUi()
        {
            image.color = Color.clear;
            keybindingText.text = "";

            if (progressBarId == null) return;

            LeanTween.cancel(progressBarId.uniqueId);
            coolDownProgressBar.Current = 0;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (abilities.Length > 0) ToolTipSystem.Instance.ShowToolTip(abilities[currentAbility]);
        }

        public void OnPointerExit(PointerEventData eventData) => ToolTipSystem.Instance.HideToolTip(ability: true);

        private string GetKeyBindingText()
        {
            InputActionReference reference = index switch
            {
                WeaponClass.AbilityIndex.Abilities1 => inputActionReferences.ability1,
                WeaponClass.AbilityIndex.Abilities2 => inputActionReferences.ability2,
                WeaponClass.AbilityIndex.Abilities3 => inputActionReferences.ability3,
                _ => inputActionReferences.ability1
            };

            int bindingIndex = reference.action.GetBindingIndexForControl(reference.action.controls[0]);

            return reference.action.bindings[bindingIndex].ToDisplayString();
        }
    }
}

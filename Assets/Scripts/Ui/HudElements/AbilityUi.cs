using Combat;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ui.HudElements
{
    [ExecuteInEditMode]
    public class AbilityUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Components")] [SerializeField]
        private Image image;

        [SerializeField] private TextMeshProUGUI keybindingText;
        [SerializeField] private GameObject comboIndicator;
        [SerializeField] private ProgressBar coolDownProgressBar;

        [Space] [Header("Private Variables")] [SerializeField]
        private AbilityBase[] abilities;

        public AbilityBase[] Abilities
        {
            set
            {
                abilities = value;

                SetAbility(0);
            }
        }

        public void SetAbility(int comboIndex)
        {
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

            var abilityBase = abilities[comboIndex];
            if (abilityBase == null)
            {
                ClearUi();
                return;
            }

            image.sprite = abilityBase.Icon;
            image.color = Color.white;
        }

        public void SetCoolDown(float amount)
        {
        }

        private void ClearUi()
        {
            image.color = Color.clear;
            keybindingText.text = "";

            coolDownProgressBar.Current = 0;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // if (abilities.Length > 0) ToolTipSystem.Instance?.ShowToolTip(abilities[currentAbility]);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // ToolTipSystem.Instance?.HideToolTip(ability: true);
        }
    }
}
using System;
using Systems;
using Combat;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Utils.Ui;

namespace Ui.ToolTip.Types
{
    public class AbilityToolTip : FollowMouse
    {
        public CanvasGroup canvasGroup;

        [SerializeField] private AbilityBase abilityBase;

        public AbilityBase AbilityBase
        {
            set
            {
                abilityBase = value;
                UpdateUi();
            }
        }

        [Space]
        [SerializeField] private Image headerImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [Space] [Header("Properties.")] [SerializeField]
        private GameObject spacer;

        [SerializeField] private GameObject propertiesContainer;
        [SerializeField] private GameObject propertyPreFab;

        private void UpdateUi()
        {
            if (abilityBase == null) return;

            headerImage.sprite = abilityBase.Icon;
            nameText.text = abilityBase.Name;

            descriptionText.text = abilityBase.Description;

            ClearChildren();
            CreateProperties(abilityBase.Properties);
        }

        private void CreateProperties(AbilityProperty[] abilityProperties)
        {
            propertiesContainer.SetActive(true);
            spacer.SetActive(true);
            foreach (AbilityProperty property in abilityProperties)
            {
                GameObject obj = Instantiate(propertyPreFab, propertiesContainer.transform);
                obj.SetActive(true);

                string text = property.IsElemental
                    ? property.Element switch
                    {
                        WeaponElement.None => "<color=#7851a9>",
                        WeaponElement.Water => "<color=#1b95e0>",
                        WeaponElement.Fire => "<color=#F73718>",
                        WeaponElement.Earth => "<color=#91672C>",
                        WeaponElement.Wind => "<color=#90ee90>",
                        _ => throw new ArgumentOutOfRangeException()
                    }
                    : "<color=#FFFFFF>";

                text += (property.IsElemental ? property.Element.ToString() : property.AttackType.ToString()) +
                        "</color> : ";

                obj.GetComponentInChildren<TextMeshProUGUI>().text = text + property.Amount;
            }
        }

        private void ClearChildren()
        {
            foreach (Transform childObj in propertiesContainer.transform)
            {
                Destroy(childObj.gameObject);
            }
        }

        protected override void Update()
        {
            if (ToolTipSystem.Instance.HideAdvanceToolTips)
                canvasGroup.alpha = 0;
            else
            {
                base.Update();
                canvasGroup.alpha = 1;
            }
        }
    }
}
using System;
using Entity.Combat;
using Entity.Combat.Abilities;
using Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Ui;

namespace Ui.ToolTip.Types
{
    public class AbilityToolTip : FollowMouse
    {
        public CanvasGroup canvasGroup;

        [SerializeField] private AbilityBase abilityBase;

        [Space] [SerializeField] private Image headerImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [Space] [Header("Properties.")] [SerializeField]
        private GameObject spacer;

        [SerializeField] private GameObject propertiesContainer;
        [SerializeField] private GameObject propertyPreFab;

        public AbilityBase AbilityBase
        {
            set
            {
                abilityBase = value;
                UpdateUi();
            }
        }

        protected override void Update()
        {
            if (ToolTipSystem.Instance.HideAdvanceToolTips)
            {
                canvasGroup.alpha = 0;
            }
            else
            {
                base.Update();
                canvasGroup.alpha = 1;
            }
        }

        private void UpdateUi()
        {
            if (abilityBase == null) return;

            headerImage.sprite = abilityBase.Icon;
            nameText.text = abilityBase.Name;

            descriptionText.text = abilityBase.Description;

            ClearChildren();
            switch (abilityBase)
            {
                case AreaOfEffectAbility attackAbility:
                    CreateProperties(attackAbility.Properties);
                    break;
                case ProjectileAbility projectileAbility:
                    CreateProperties(projectileAbility.Properties);
                    break;
            }
        }

        private void CreateProperties(AbilityProperty[] abilityProperties)
        {
            propertiesContainer.SetActive(true);
            spacer.SetActive(true);
            foreach (var property in abilityProperties)
            {
                var obj = Instantiate(propertyPreFab, propertiesContainer.transform);
                obj.SetActive(true);

                var text = property.IsElemental
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
            foreach (Transform childObj in propertiesContainer.transform) Destroy(childObj.gameObject);

            propertiesContainer.SetActive(false);
            spacer.SetActive(false);
        }
    }
}
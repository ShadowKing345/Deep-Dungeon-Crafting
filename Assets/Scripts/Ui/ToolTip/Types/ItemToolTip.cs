using System;
using System.Globalization;
using Systems;
using Combat;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Ui;

namespace Ui.ToolTip.Types
{
    public class ItemToolTip : FollowMouse
    {
        public CanvasGroup canvasGroup;

        [SerializeField] private ItemStack stack;

        public ItemStack ItemStack
        {
            get => stack;
            set
            {
                stack = value;
                UpdateUi();
            }
        }

        [Space] [SerializeField] private Image headerImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [Space] [Header("Properties.")] [SerializeField]
        private GameObject spacer;

        [SerializeField] private GameObject propertiesContainer;
        [SerializeField] private GameObject propertyPreFab;
        
        private void UpdateUi()
        {
            if (stack.IsEmpty) return;

            headerImage.sprite = stack.Item.Icon;
            nameText.text = stack.Item.name;
            amountText.text = stack.Amount > 0 ? stack.Amount.ToString() : string.Empty;

            descriptionText.text = stack.Item.Description;

            ClearChildren();
            switch (stack.Item)
            {
                case ArmorItem armorItem when armorItem.properties.Length > 0:
                    CreateProperties(armorItem.properties, true);
                    break;
                case WeaponItem weaponItem when weaponItem.Properties.Length > 0:
                    CreateProperties(weaponItem.Properties);
                    break;
                default:
                    propertiesContainer.gameObject.SetActive(false);
                    spacer.SetActive(false);
                    break;
            }
        }

        private void CreateProperties(AbilityProperty[] abilityProperties, bool asPercentages = false)
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

                obj.GetComponentInChildren<TextMeshProUGUI>().text = text + (asPercentages ? (property.Amount * 100) + "%" : property.Amount.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void ClearChildren()
        {
            foreach (Transform childObj in propertiesContainer.transform)
            {
                Destroy(childObj.gameObject);
            }
            
            propertiesContainer.SetActive(false);
            spacer.SetActive(false);
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
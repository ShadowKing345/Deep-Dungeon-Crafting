using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace Ui
{
    public class WeaponAbilityUi : MonoBehaviour
    {
        public WeaponAbility ability;
        public Image abilityImage;
        public Button executeButton;
        public TextMeshProUGUI keybinding;
        
        public void SetAbility(WeaponAbility ability)
        {
            this.ability = ability;
            if (ability == null)
            {
                abilityImage.sprite = null;
                abilityImage.color = Color.clear;
            }
            else
            {
                abilityImage.sprite = ability.icon;
                abilityImage.color = Color.white;
            }
        }
    }
}

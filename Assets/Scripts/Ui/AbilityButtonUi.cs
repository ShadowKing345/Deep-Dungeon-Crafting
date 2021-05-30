using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace Ui
{
    public class AbilityButtonUi : MonoBehaviour
    {
        public WeaponAbility ability;
        private bool _isAbilityNull = true;
        public Image abilityImage;
        public Button executeButton;
        public TextMeshPro keybinding;

        public void Start()
        {
            abilityImage ??= GameObject.Find("ProgressBar").GetComponent<Image>();
            executeButton ??= GameObject.Find("Button").GetComponent<Button>();
            keybinding ??= GameObject.Find("Keybinding").GetComponent<TextMeshPro>();

            if (ability.isNull) _isAbilityNull = false;
            UpdateUi();
        }

        public void UpdateUi()
        {
            if (_isAbilityNull) return;

            abilityImage.sprite = ability.icon;
        }
        
        public void SetAbility(WeaponAbility ability){}
    }
}

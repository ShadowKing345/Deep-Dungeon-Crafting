using Combat;
using Entity.Player;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Ui.HudElements
{
    public class AbilityUi : MonoBehaviour, IHudElement
    {
        [SerializeField] private Ability ability;
        [SerializeField] private Image abilityImage;
        [SerializeField] private Button executeButton;
        [SerializeField] private TextMeshProUGUI keybindingText;
        [SerializeField] private PlayerCombat playerCombat;
        // public InputManager.KeyValue KeyValue { get; set; } = InputManager.KeyValue.None;
        public WeaponClass.AbilityIndex AbilityIndex { get; set; }

        private void Start()
        {
            playerCombat ??= FindObjectOfType<PlayerCombat>();
            executeButton.onClick.AddListener(Attack);
        }

        public Ability Ability
        {
            get => ability;
            set { ability = value; UpdateUi(); }
        }

        public void UpdateUi()
        {
            if (ability == null)
            {
                abilityImage.sprite = null;
                abilityImage.color = Color.clear;
                keybindingText.text = "";
            }
            else
            {
                abilityImage.sprite = ability.Icon;
                abilityImage.color = Color.white;
                
                // KeyCode code = InputManager.instance.GetCodeFromValue(KeyValue);
                // keybindingText.text = code == KeyCode.None ? "" : code.ToString();
            }
        }
        
        private void Attack()
        {
            if (playerCombat == null) return;
            playerCombat.Attack(AbilityIndex);
        }
    }
}

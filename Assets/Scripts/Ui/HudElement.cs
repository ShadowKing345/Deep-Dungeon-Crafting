using System;
using System.Collections.Generic;
using Combat;
using Entity.Player;
using TMPro;
using Ui.HudElements;

namespace Ui
{
    [Serializable]
    public class HudElement
    {
        public ProgressBar healthProgressBar;
        public ProgressBar manaProgressBar;

        public AbilityUi abilityUi1;
        public AbilityUi abilityUi2;
        public AbilityUi abilityUi3;

        public TextMeshProUGUI floorNumber;
        
        public bool TryGetAbility(WeaponClass.AbilityIndex index, out AbilityUi abilityUi)
        {
            abilityUi = index switch
            {
                WeaponClass.AbilityIndex.Abilities1 => abilityUi1,
                WeaponClass.AbilityIndex.Abilities2 => abilityUi2,
                WeaponClass.AbilityIndex.Abilities3 => abilityUi3,
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
            return abilityUi != null;
        }
        
        public void SetMaxHealthMana(float health, float mana)
        {
            healthProgressBar.MAX = health;
            manaProgressBar.MAX = mana;
        }

        public void SetHealthMana(float health, float mana)
        {
            healthProgressBar.Current = health;
            manaProgressBar.Current = mana;
        }

        public void InitializeAbilityUi(PlayerCombat combatController, Dictionary<WeaponClass.AbilityIndex, AbilityBase[]> dictionary)
        {
            foreach (KeyValuePair<WeaponClass.AbilityIndex,AbilityBase[]> kvPair in dictionary)
                if (TryGetAbility(kvPair.Key, out AbilityUi abilityUi))
                    abilityUi.SetUp(combatController, kvPair.Key, kvPair.Value);
        }

        public void SetAbilityUiComboIndex(WeaponClass.AbilityIndex index, int comboIndex)
        {
            if(TryGetAbility(index, out AbilityUi ui)) ui.SetAbility(comboIndex);
        }

        public void SetAbilityUiCoolDown(WeaponClass.AbilityIndex index, float amount)
        {
            if(TryGetAbility(index, out AbilityUi ui)) ui.SetCoolDown(amount);
        }
    }
}
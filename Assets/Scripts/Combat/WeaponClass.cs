using System;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(menuName = "SO/Weapon Class", fileName = "New Weapon Class SO")]
    [Serializable]
    public class WeaponClass : ScriptableObject
    {
        [TextArea(3, 3)] [SerializeField] private string shortDescription;
        [TextArea(7, 7)] [SerializeField] private string longDescription;
        [SerializeField] private Sprite icon;

        public string ShortDescription => shortDescription;
        public string LongDescription => longDescription;
        public Sprite Icon => icon;

        [SerializeField] private AbilityBase[] abilities1 = Array.Empty<AbilityBase>();
        [SerializeField] private AbilityBase[] abilities2 = Array.Empty<AbilityBase>();
        [SerializeField] private AbilityBase[] abilities3 = Array.Empty<AbilityBase>();

        public AbilityBase[] GetAbility(AbilityIndex abilityIndex) =>
            abilityIndex switch
            {
                AbilityIndex.Abilities1 => abilities1,
                AbilityIndex.Abilities2 => abilities2,
                AbilityIndex.Abilities3 => abilities3,
                _ => throw new ArgumentOutOfRangeException(nameof(abilityIndex), abilityIndex, null)
            };

        public enum AbilityIndex
        {
            Abilities1,
            Abilities2,
            Abilities3
        }

        public AbilityBase[][] Abilities => new[] { abilities1, abilities2, abilities3 };
    }
}
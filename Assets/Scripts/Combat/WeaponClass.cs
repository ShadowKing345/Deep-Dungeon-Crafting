using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(menuName = "SO/Weapon Class", fileName = "New Weapon Class SO")]
    [System.Serializable]
    public class WeaponClass : ScriptableObject
    {
        [TextArea(3,3)]
        [SerializeField] private string shortDescription;
        [TextArea(7,7)]
        [SerializeField] private string longDescription;
        [SerializeField] private Sprite icon;

        public string ShortDescription => shortDescription;
        public string LongDescription => longDescription;
        public Sprite Icon => icon;

        [SerializeField] private Ability[] abilities1 = new Ability[0];
        [SerializeField] private Ability[] abilities2 = new Ability[0];
        [SerializeField] private Ability[] abilities3 = new Ability[0];

        public Ability[] GetAbility(AbilityIndex abilityIndex) =>
            abilityIndex switch
            {
                AbilityIndex.Abilities1 => abilities1,
                AbilityIndex.Abilities2 => abilities2,
                AbilityIndex.Abilities3 => abilities3,
                _ => null
            };

        public enum AbilityIndex
        {
            Abilities1,
            Abilities2,
            Abilities3
        }

        public List<Ability[]> Abilities => new List<Ability[]>(new[] {abilities1, abilities2, abilities3});
    }
}
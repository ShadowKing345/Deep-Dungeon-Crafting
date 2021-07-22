using System.Linq;
using UnityEngine;

namespace Combat.Buffs
{
    [CreateAssetMenu(fileName = "New Defensive Buff", menuName = "SO/Combat/Buffs/Defensive Buff")]
    public class DefensiveBuff: BuffBase
    {
        [SerializeField] private AbilityProperty[] properties;

        public AbilityProperty[] Properties => properties;

        public bool HasResistanceTo(AbilityProperty property) =>
            properties.FirstOrDefault(p =>
                p.IsElemental ? p.Element == property.Element : p.AttackType == property.AttackType) != null;

        public override void Tick() { }
    }
}
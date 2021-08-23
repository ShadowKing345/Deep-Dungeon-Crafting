using System.Linq;
using UnityEngine;

namespace Combat.Buffs
{
    [CreateAssetMenu(fileName = "New Defensive Buff", menuName = "SO/Combat/Buffs/Defensive Buff")]
    public class DefensiveBuff: BuffBase
    {
        [SerializeField] private AbilityProperty[] properties;

        public AbilityProperty[] Properties => properties;
        
        public override void Tick() { }
    }
}
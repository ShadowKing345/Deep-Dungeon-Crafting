using Project.Runtime.Utils.Interfaces;
using UnityEngine;

namespace Project.Runtime.Entity.Combat.Abilities
{
    [CreateAssetMenu(fileName = "New Buff Ability", menuName = "SO/Combat/Ability/Buff Ability")]
    public class BuffAbility : AbilityBase
    {
        [SerializeField] private BuffBase[] buffs;
        [SerializeField] private float[] durations;

        public BuffBase[] Buffs => buffs;
        public float[] Durations => durations;

        public override bool Execute(IDamageable self)
        {
            return true;
        }
    }
}
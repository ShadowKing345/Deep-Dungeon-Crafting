using Combat.Buffs;
using Interfaces;
using UnityEngine;

namespace Combat.Abilities
{
    [CreateAssetMenu(fileName = "New Buff Ability", menuName = "SO/Combat/Ability/Buff Ability")]
    public class BuffAbility : AbilityBase
    {
        [SerializeField] private BuffBase[] buffs;
        [SerializeField] private float[] durations;

        public BuffBase[] Buffs => buffs;
        public float[] Durations => durations;

        public override bool Execute(IDamageable self, IDamageable[] targets)
        {
            // if(!base.Execute(self, targets)) return false;
            var flag = false;

            for (int i = 0; i < buffs.Length; i++)
            {
                if (buffs[i].ForSelf)
                {
                    flag = self.Buff(buffs[i], durations[i]);
                }
                else
                {
                    foreach (var target in targets)
                    {
                        if (target.Buff(buffs[i], durations[i]))
                            flag = true;
                    }
                }
            }

            return flag;
        }
    }
}
using System;

namespace Project.Runtime.Entity.Combat.Buffs
{
    [Serializable]
    public class ActiveBuff
    {
        public BuffBase buff;
        public float duration;
    }
}
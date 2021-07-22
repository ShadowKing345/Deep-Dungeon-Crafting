using System;

namespace Combat.Buffs
{
    [Serializable]
    public class ActiveBuff
    {
        public BuffBase buff;
        public float duration;
    }
}
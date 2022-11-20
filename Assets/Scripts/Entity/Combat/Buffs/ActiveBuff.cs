using System;

namespace Entity.Combat.Buffs
{
    [Serializable]
    public class ActiveBuff
    {
        public BuffBase buff;
        public float duration;
    }
}
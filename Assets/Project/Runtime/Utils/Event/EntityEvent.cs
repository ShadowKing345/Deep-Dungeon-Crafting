using System;

namespace Utils.Event
{
    [Serializable]
    public class EntityEvent
    {
        public EntityEventType Type { get; set; }
        public double Value { get; set; }
    }

    public enum EntityEventType
    {
        Death,
        Healing,
        Damage,
        Buff
    }
}
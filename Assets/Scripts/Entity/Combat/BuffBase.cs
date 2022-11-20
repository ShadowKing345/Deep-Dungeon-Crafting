using UnityEngine;

namespace Entity.Combat
{
    public abstract class BuffBase : ScriptableObject
    {
        [Multiline] [SerializeField] private string description;

        [SerializeField] private Sprite icon;

        [SerializeField] private bool forSelf;
        [SerializeField] private bool isBeneficial;

        public string Description => description;
        public Sprite Icon => icon;

        public bool ForSelf => forSelf;
        public bool IsBeneficial => isBeneficial;

        public abstract void Tick();
    }
}
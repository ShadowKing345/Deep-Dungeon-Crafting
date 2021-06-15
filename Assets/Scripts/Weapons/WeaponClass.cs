using System.Linq;
using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(menuName = "SO/Weapon Class", fileName = "New Weapon Class SO")]
    [System.Serializable]
    public class WeaponClass : ScriptableObject
    {
        public string description;
        public Sprite icon;

        public WeaponAbility[] action1 = new WeaponAbility[0];
        public WeaponAbility[] action2 = new WeaponAbility[0];
        public WeaponAbility[] action3 = new WeaponAbility[0];

        public WeaponAbility[] GetActionOfIndex(int index) =>
            index switch
            {
                1 => action1,
                2 => action2,
                3 => action3,
                _ => null
            };
        public WeaponAbility[] Actions => (WeaponAbility[]) action1.Concat(action2.Concat(action3));
    }
}
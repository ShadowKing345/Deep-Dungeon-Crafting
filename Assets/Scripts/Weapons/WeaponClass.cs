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

        public WeaponAction[] action1 = new WeaponAction[0];
        public WeaponAction[] action2 = new WeaponAction[0];
        public WeaponAction[] action3 = new WeaponAction[0];

        public WeaponAction[] GetActionOfIndex(int index)
        {
            return index switch
            {
                1 => action1,
                2 => action2,
                3 => action3,
                _ => null
            };
        }
        public WeaponAction[] Actions => (WeaponAction[]) action1.Concat(action2.Concat(action3));
    }
}
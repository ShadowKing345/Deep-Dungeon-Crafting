using UnityEngine;
using UnityEngine.UI;

namespace Weapons
{
    [CreateAssetMenu(fileName = "WeaponClass", menuName = "SO/Weapon Class")]
    public class WeaponClass : ScriptableObject
    {
        public string name;
        public string description;
        public Image classIcon;

        public WeaponAction[] action1;
        public WeaponAction[] action2;
        public WeaponAction[] action3;
    }
}
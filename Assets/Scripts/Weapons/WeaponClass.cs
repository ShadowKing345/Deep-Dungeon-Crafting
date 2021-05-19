using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Weapons
{
    [Serializable]
    public class WeaponClass
    {
        public string name;
        public string description;
        public Image classIcon;

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

        // public WeaponAction[] actions => (WeaponAction[]) action1.Concat(action2.Concat(action3));
    }
}
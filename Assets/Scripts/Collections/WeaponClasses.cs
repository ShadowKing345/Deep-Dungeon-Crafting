using System;
using UnityEngine;
using Weapons;

namespace Collections
{
    [CreateAssetMenu(fileName = "WeaponClasses", menuName = "SO/Weapon Classes")]
    public class WeaponClasses : ScriptableObject
    {
        public WeaponClass[] gameData;
    }
}
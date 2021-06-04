using System;
using UnityEngine;
using Weapons;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public WeaponClass noWeaponClass;

        private void OnEnable()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
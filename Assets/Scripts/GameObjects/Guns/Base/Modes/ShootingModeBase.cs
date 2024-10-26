
using UnityEngine;

namespace GameObjects.Guns.Base.Modes
{
    public class ShootingModeBase : ScriptableObject
    {
        public string modeName;
        
        [Header("Reloading")]
        public float fireRate;
        public float reloadTime;
        
    }
}
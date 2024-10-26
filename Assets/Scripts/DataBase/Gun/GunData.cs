using DataBase.Ammo;
using UnityEngine;

namespace DataBase.Gun
{
    [CreateAssetMenu(fileName = "GunData", menuName = "Gun/GunData", order = 0)]
    public class GunData : ScriptableObject
    {
        [Header("Info")] 
        public string gunName;

        [Header("Shooting")]
        public float damage;

        [Header("Ammo")] 
        public AmmoData ammoData;
        public int magSize;
        public int currentAmountOfAmmo;
        
        [Header("Reloading")]
        public float fireRate;
        public float reloadTime;
        
        [Header("State")]
        public bool reloading;
        public float timeSinceLastShot;

        public LayerMask shootingLayers;
        
    }
}
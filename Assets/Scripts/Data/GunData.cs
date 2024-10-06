using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "GunData", menuName = "Gun/GunData", order = 0)]
    public class GunData : ScriptableObject
    {
        [Header("Info")] 
        public string gunName;

        [Header("Shooting")]
        public float damage;
        public float maxDistance;
        
        [Header("Ammo")]
        public int currentAmmo;
        public int magSize;
        public GameObject ammo;
        public float ammoSpeed;
        
        [Header("Reloading")]
        public float fireRate;
        public float reloadTime;
        
        [Header("State")]
        public bool reloading;
        public float timeSinceLastShot;

        public LayerMask shootingLayers;

    }
}
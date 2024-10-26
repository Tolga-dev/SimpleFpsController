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
        
        [Header("Recoil")]
        public Vector3 recoilBack = new Vector3(0, 0, -0.1f); // Amount gun moves back
        public float returnSpeed = 20f; // Speed of recoil return
        
        public float recoilAmount = 5f;          // Base vertical recoil intensity
        public float horizontalRecoilAmount = 2f; // Base horizontal recoil intensity
        public float recoilRecovery = 2f;        // Speed of recovery
    }
}
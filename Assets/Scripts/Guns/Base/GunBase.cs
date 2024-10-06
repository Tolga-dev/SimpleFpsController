using System;
using System.Collections;
using Ammo.Base;
using Data;
using UnityEngine;

namespace Guns.Base
{
    public class GunBase : MonoBehaviour
    {
        [Header("Gun Data")]
        public GunData gunData;
        
        [Header("Gun Transforms")]
        public Transform shootingSpawnPoint;
        public Transform holdPlacePoint;

        [Header("Ammo")] 
        public AmmoBase ammoBase;
        
        private void Start()
        {
            ammoBase = GetAmmoType();
        }

        private void Update()
        {
            gunData.timeSinceLastShot += Time.deltaTime;

            GunShootingAction();

            if (Input.GetKeyDown(KeyCode.R))
                ReloadGun();
            
        }
        private void ReloadGun()
        {
            if (!gunData.reloading && gameObject.activeSelf)
                StartCoroutine(Reload());
        }
        
        private void GunShootingAction()
        {
            if (gunData.currentAmmo <= 0) return;
            if (!CanShoot()) return;
            
            SpawnAmmo();
            
            gunData.currentAmmo--;
            gunData.timeSinceLastShot = 0;
        }
        private void SpawnAmmo()
        {
            if (ammoBase == null)
            {
                Debug.LogError("AmmoBase is not assigned!");
                return;
            }

            var spawnedAmmo = Instantiate(ammoBase, shootingSpawnPoint.position, shootingSpawnPoint.rotation);

            if (spawnedAmmo.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.AddForce(transform.forward * gunData.ammoSpeed, ForceMode.Impulse);
            }
        }
     
        private AmmoBase GetAmmoType()
        {
            return gunData.ammo.GetComponent<AmmoBase>();
        }
        private bool CanShoot() => !gunData.reloading && gunData.timeSinceLastShot > gunData.fireRate;
        
        private IEnumerator Reload() {
            gunData.reloading = true;

            yield return new WaitForSeconds(gunData.reloadTime);

            gunData.currentAmmo = gunData.magSize;

            gunData.reloading = false;
        }
    }
}
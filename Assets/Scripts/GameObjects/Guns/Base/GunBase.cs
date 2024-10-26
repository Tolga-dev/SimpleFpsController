using System.Collections;
using DataBase.Gun;
using UnityEngine;

namespace GameObjects.Guns.Base
{
    public class GunBase : ObjectBase.ObjectBase
    {
        [Header("Data")]
        public GunData gunData;

        public Transform shootingSpawnPoint;

        private void Update()
        {
            gunData.timeSinceLastShot += Time.deltaTime;
            
            GunShootingAction();

            if (Input.GetKeyDown(KeyCode.R))
                ReloadGun();
            
        }
        private void ReloadGun()
        {
            if (!gunData.reloading) StartCoroutine(Reload());
        }
        
        private void GunShootingAction()
        {
            if (!CanShoot()) return;
            
            SpawnAmmo();
            
            gunData.currentAmountOfAmmo--;
            gunData.timeSinceLastShot = 0;
        }
        private void SpawnAmmo()
        {
            var spawnedAmmo = Instantiate(gunData.ammoData.ammo, shootingSpawnPoint.position,
                shootingSpawnPoint.rotation);
            
            if (spawnedAmmo.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.AddForce(transform.forward * gunData.ammoData.ammoSpeed, ForceMode.Impulse);
            }
            
        }
        private bool CanShoot() => 
            gunData.reloading == false && 
            gunData.timeSinceLastShot > gunData.fireRate && 
            gunData.currentAmountOfAmmo > 0;
        
        private IEnumerator Reload() {
            gunData.reloading = true;

            yield return new WaitForSeconds(gunData.reloadTime);

            gunData.currentAmountOfAmmo = gunData.magSize;

            gunData.reloading = false;
        }
    }
}
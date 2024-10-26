using GameObjects.Ammo.Base;
using UnityEngine;

namespace GameObjects.Guns.Base.States
{
    public class FireState : BaseState
    {
        public FireState(GunBase gunBase) : base(gunBase)
        {
        }

        public override void OnStateEnter()
        {
            gunData.timeSinceLastShot = 0; // hope it will not be problem in future
        }

        public override void OnStateUpdate()
        {
            gunData.timeSinceLastShot += Time.deltaTime;
            GunShootingAction();
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
            var spawnedAmmo = Object.Instantiate(gunData.ammoData.ammo, gunBase.shootingSpawnPoint.position,
                gunBase.shootingSpawnPoint.rotation);
            var ammoBase = spawnedAmmo.GetComponent<AmmoBaseInGame>();
            
            ammoBase.ammoData = gunData.ammoData;
            ammoBase.gunDamage = gunData.damage;
            ammoBase.rb.AddForce(gunBase.transform.forward * gunData.ammoData.ammoSpeed, ForceMode.Impulse);
        }
        private bool CanShoot() => 
            gunData.reloading == false && 
            gunData.timeSinceLastShot > gunData.fireRate && 
            gunData.currentAmountOfAmmo > 0; 
    }
}
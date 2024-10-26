using System;
using GameObjects.Ammo.Base;
using GameObjects.Guns.Base.Modes;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Single = GameObjects.Guns.Base.Modes.Single;

namespace GameObjects.Guns.Base.States
{
    [Serializable]
    public class FireState : BaseState
    {
        // recoil
        private Vector3 originalPosition;
        private float currentRecoilX = 0f;       // Current horizontal recoil
        private float currentRecoilY = 0f;       // Current vertical recoil
 
        public float heatPerShot = 10f;       // Heat added per shot
        public float maxHeat = 100f;          // Max heat before gun jams or overheats
        public float coolingRate = 5f;        // Rate at which heat dissipates
        private float currentHeat = 0f;       // Current heat level
        private bool isOverheated = false;    // Indicates if the gun is overheated

        public override void Starter(GunBase worldGunBase)
        {
            base.Starter(worldGunBase);
            originalPosition = gunBase.transform.localPosition;
        }

        public override void OnStateEnter()
        {
            gunData.timeSinceLastShot = gunData.GetCurrentMode().fireRate - Time.deltaTime; // hope it will not be problem in future
        }

        public override void OnStateUpdate()
        {
            gunData.timeSinceLastShot += Time.deltaTime;
            GunShootingAction();
            MoveToOriginalPositionAfterRecoil();
        }

        public override void OnStateExit()
        {
            var currentTransform = gunBase.transform;
            currentTransform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        private void MoveToOriginalPositionAfterRecoil()
        {
            var currentTransform = gunBase.transform;

            if (currentTransform.localPosition != originalPosition)
            {
                currentTransform.localPosition = Vector3.Lerp(currentTransform.localPosition, originalPosition,
                    Time.deltaTime * gunData.returnSpeed);
            }

            if (currentRecoilY != 0 || currentRecoilX != 0)
            {
                currentRecoilY = Mathf.Lerp(currentRecoilY, 0f, Time.deltaTime * gunData.recoilRecovery);
                currentRecoilX = Mathf.Lerp(currentRecoilX, 0f, Time.deltaTime * gunData.recoilRecovery);
                currentTransform.localRotation = Quaternion.Euler(-currentRecoilY, currentRecoilX, 0);
            }
            
        }

        private void GunShootingAction()
        {
            if (!CanShoot()) return;
            
            SpawnAmmo();
            ApplyRecoil();
            Shoot();
            
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
            gunData.timeSinceLastShot > gunData.GetCurrentMode().fireRate && 
            gunData.currentAmountOfAmmo > 0;

        private void ApplyRecoil()
        {
            gunBase.transform.localPosition += gunData.recoilBack;
         
            currentRecoilY += Random.Range(gunData.recoilAmount * 0.8f, gunData.recoilAmount * 1.2f); // Vertical recoil
            currentRecoilX += Random.Range(-gunData.horizontalRecoilAmount, gunData.horizontalRecoilAmount); // Horizontal recoil
        }
        private void Shoot()
        {
            if (currentHeat > 0 && !isOverheated)
            {
                currentHeat -= coolingRate * Time.deltaTime;
                currentHeat = Mathf.Clamp(currentHeat, 0, maxHeat); // Keep within bounds
            }
            else if (isOverheated && currentHeat <= maxHeat * 0.3f) // 30% threshold to unjam
            {
                isOverheated = false; // Unjam when sufficiently cooled down
                Debug.Log("Gun is ready to fire again!");
            }
            
            if (isOverheated)
            {
                Debug.Log("Gun is overheated! Wait for it to cool down.");
                return; // Prevent shooting when overheated
            }

            Debug.Log("Bang!");

            currentHeat += heatPerShot;
            if (currentHeat >= maxHeat)
            {
                isOverheated = true;
                Debug.Log("Gun has overheated!");
            }
        }

        public void SwitchToNextShootMode()
        {
            gunData.SwitchAnotherMode();
        }
        
 
    }
}
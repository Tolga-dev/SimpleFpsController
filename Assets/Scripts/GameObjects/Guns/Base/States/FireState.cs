using System;
using GameObjects.Ammo.Base;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace GameObjects.Guns.Base.States
{
    [Serializable]
    public class FireState : BaseState
    {
        // recoil
        private Vector3 originalPosition;
        private float currentRecoilX = 0f;       // Current horizontal recoil
        private float currentRecoilY = 0f;       // Current vertical recoil
        
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

            if (gunData.isOverheated || gunData.isJammed)
            {
                gunBase.SwitchNewState(gunBase.idleState);
                return;
            }
            
            SpawnAmmo();
            ApplyRecoil();

        }
        private void SpawnAmmo()
        {
            var spawnedAmmo = Object.Instantiate(gunData.ammoData.ammo, gunBase.shootingSpawnPoint.position,
                gunBase.shootingSpawnPoint.rotation);
            var ammoBase = spawnedAmmo.GetComponent<AmmoBaseInGame>();
            
            ammoBase.ammoData = gunData.ammoData;
            ammoBase.gunDamage = gunData.damage;
            ammoBase.rb.AddForce(gunBase.transform.forward * gunData.ammoData.ammoSpeed, ForceMode.Impulse);
            
            gunData.currentHeat += gunData.heatPerShot;
            if (gunData.currentHeat >= gunData.maxHeat)
            {
                gunData.amountOfOverHeatTimes++;
                gunData.isOverheated = true;
                Debug.Log("Gun has overheated!");
                
            }

            if (gunData.maxOverHeatTimeForJamChecking < gunData.amountOfOverHeatTimes)
            {
                gunData.amountOfOverHeatTimes = 0;
                if (Random.value < gunData.jamProbability)
                {
                    gunData.isJammed = true;
                    gunData.currentJammedTime = gunData.jamFixTime;
                    
                    Debug.Log("Gun has jammed!");
                    return;
                }               
            }
                        
            gunData.currentAmountOfAmmo--;
            gunData.timeSinceLastShot = 0;
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

        public void SwitchToNextShootMode()
        {
            gunData.SwitchAnotherMode();
        }
        
 
    }
}
using System.Collections;
using UnityEngine;

namespace GameObjects.Guns.Base.States
{
    public class ReloadState : BaseState
    {
        public ReloadState(GunBase gunBase) : base(gunBase)
        {
            
        }
        
        public override void OnStateEnter()
        {
            ReloadGun();
        }
        
        private void ReloadGun()
        {
            if (!gunData.reloading) gameManager.StartCoroutine(Reload());
        }
        private IEnumerator Reload() {
            gunData.reloading = true;

            yield return new WaitForSeconds(gunData.reloadTime);

            gunData.currentAmountOfAmmo = gunData.magSize;

            gunData.reloading = false;
        }
        
    }
}
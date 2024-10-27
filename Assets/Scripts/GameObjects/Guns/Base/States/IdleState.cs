using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameObjects.Guns.Base.States
{
    [Serializable]
    public class IdleState :  BaseState
    {
        public override void OnStateUpdate()
        {
            Jamming();
            CoolDown();
        }

        private void Jamming()
        {
            if (gunData.isJammed)
            {
                gunData.currentJammedTime -= gunData.jamFixTimeRate * Time.deltaTime;

                if (gunData.currentJammedTime <= 0)
                    gunData.isJammed = false;
            }
        }

        public void CoolDown()
        {
            switch (gunData.isOverheated)
            {
                case false:
                    if (gunData.currentHeat == 0) 
                        break;
                    gunData.currentHeat = Mathf.Clamp(gunData.currentHeat, 0, gunData.maxHeat); // Keep within bounds
                    break;
                case true when gunData.currentHeat <= gunData.maxHeat * 0.3:
                    gunData.isOverheated = false;
                    Debug.Log("Gun is ready to fire again!");
                    break;
            }
            
            if(gunData.currentHeat > 0)
                gunData.currentHeat -= gunData.coolingRate * Time.deltaTime;
        }
        
    }
}
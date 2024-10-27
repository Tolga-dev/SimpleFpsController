using System;
using UnityEngine;

namespace GameObjects.Guns.Base.States
{
    [Serializable]
    public class IdleState :  BaseState
    {
        public override void OnStateUpdate()
        {
            CoolDown();
        }
        
        public void CoolDown()
        {
            switch (gunData.isOverheated)
            {
                case false:
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
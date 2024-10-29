using UnityEngine;

namespace GameObjects.Guns.Base
{
    public class GunInput
    {
        public GunBase currentGunBase;

        public GunInput(GunBase gunBase)
        {
            currentGunBase = gunBase;
        }

        public void GunUpdateInput()
        {
            if (Input.GetKeyDown(KeyCode.R))
                currentGunBase.SwitchNewState(currentGunBase.reloadState);
            else if (Input.GetKeyDown(KeyCode.Alpha1))
                currentGunBase.SwitchToNextShootMode();
            
            if (Input.GetKey(KeyCode.Mouse0))
                currentGunBase.AttackInputMode();
            else
                currentGunBase.NonInputMode();
        }
        
    }
    
}

  
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
            if (Input.GetKeyDown(KeyCode.Mouse0))
                currentGunBase.SwitchNewState(currentGunBase.fireState);
            else if (Input.GetKeyUp(KeyCode.Mouse0))
                currentGunBase.SwitchNewState(currentGunBase.idleState);
            
            if (Input.GetKeyDown(KeyCode.R))
                currentGunBase.SwitchNewState(currentGunBase.reloadState);
        }
        
    }

}
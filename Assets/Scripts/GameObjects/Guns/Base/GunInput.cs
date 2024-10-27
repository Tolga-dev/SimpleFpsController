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
            if (Input.anyKey == false)
                currentGunBase.NonInputMode();

            if (Input.GetKeyDown(KeyCode.Mouse0))
                currentGunBase.AttackInputMode();

            if (Input.GetKeyDown(KeyCode.R))
                currentGunBase.SwitchNewState(currentGunBase.reloadState);
            else if (Input.GetKeyDown(KeyCode.A))
                currentGunBase.SwitchToNextShootMode();
        }
    }
    
}

  
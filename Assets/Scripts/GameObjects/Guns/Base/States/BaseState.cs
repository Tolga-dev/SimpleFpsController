using DataBase.Gun;
using Manager;
using UnityEngine;

namespace GameObjects.Guns.Base.States
{
    public class BaseState
    {
        protected GameManager gameManager;
        protected GunBase gunBase;
        protected GunData gunData;
        
        public BaseState(GunBase gunBase)
        {
            this.gunBase = gunBase;
            gameManager = gunBase.gameManager;
            gunData = gunBase.gunData;
        }

        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }
        public virtual void OnStateUpdate() { }
    }
}
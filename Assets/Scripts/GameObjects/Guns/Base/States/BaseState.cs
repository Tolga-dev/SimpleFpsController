using System;
using DataBase.Gun;
using Manager;
using UnityEngine;

namespace GameObjects.Guns.Base.States
{
    [Serializable]
    public class BaseState
    {
        protected GameManager gameManager;
        protected GunBase gunBase;
        protected GunData gunData;
        
        public virtual void Starter(GunBase gunBase1)
        {
            gunBase = gunBase1;
            gameManager = gunBase.gameManager;
            gunData = gunBase.gunData;
        }

        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }
        public virtual void OnStateUpdate() { }
    }
}
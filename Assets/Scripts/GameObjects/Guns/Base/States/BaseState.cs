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
        protected GunsDataBase gunsDataBase;
        
        public virtual void Starter(GunBase gunBase1)
        {
            gunBase = gunBase1;
            gameManager = gunBase.gameManager;
            gunData = gunBase.gunData;
            gunsDataBase = gunBase1.gameManager.dataBaseManager.gunsDataBase;
        }

        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }
        public virtual void OnStateUpdate() { }
    }
}
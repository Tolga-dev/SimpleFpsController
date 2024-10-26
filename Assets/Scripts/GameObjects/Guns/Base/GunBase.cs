using System;
using System.Collections;
using DataBase.Gun;
using GameObjects.Ammo.Base;
using GameObjects.Guns.Base.States;
using Manager;
using UnityEngine;

namespace GameObjects.Guns.Base 
{
    [Serializable]
    public class GunBase : ObjectBase.ObjectBase
    {
        [Header("States")]
        public FireState fireState;
        public ReloadState reloadState;
        public IdleState idleState;
        public BaseState baseState;
        
        [Header("Data")]
        public GunData gunData;

        public GunInput gunInput;
        
        public Transform shootingSpawnPoint;
    
        private void Start()
        {
            fireState.Starter(this);
            reloadState.Starter(this);
            idleState.Starter(this);
            gunInput = new GunInput(this);
            
            SwitchNewState(idleState);
        }

        private void Update()
        {
            gunInput.GunUpdateInput();
            baseState.OnStateUpdate();
        }
        
        public void SwitchNewState(BaseState newState)
        {
            baseState?.OnStateExit();
            baseState = newState;
            baseState.OnStateEnter();
        }
        
    }
}
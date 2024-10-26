using System;
using DataBase.Gun;
using GameObjects.Guns.Base.Modes;
using GameObjects.Guns.Base.States;
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

        public Transform shootingSpawnPoint;
        
        public GunInput gunInput;
        
        
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

        public void SwitchToNextShootMode()
        {
            fireState.SwitchToNextShootMode();
        }

        public void NonInputMode()
        {
            SwitchNewState(idleState);
        }

        public void AttackInputMode()
        {
            SwitchNewState(fireState);

        }
    }
}
using System;
using UnityEngine;

namespace GameObjects.Player.Movement.States
{
    [Serializable]
    public class PlayerSprintState : MovementStateBase
    {
        [SerializeField] private float runMultiplier = 2;

        public override void Update()
        {
            movementController.move *= runMultiplier;
        }
        
    }
}
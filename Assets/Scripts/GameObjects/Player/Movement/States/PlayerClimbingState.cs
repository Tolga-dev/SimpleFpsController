using System;
using UnityEngine;

namespace GameObjects.Player.Movement.States
{
    [Serializable]
    public class PlayerClimbingState : MovementStateBase
    {
        public float climbSpeed = 3f;

        public override void Update()
        {
            movementController.yVelocity += inputController.z * climbSpeed * Time.deltaTime;
            movementController.move.z = 0; 
        }
    }
}
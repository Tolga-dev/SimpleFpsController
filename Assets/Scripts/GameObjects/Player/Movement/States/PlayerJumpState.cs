using System;
using UnityEngine;

namespace GameObjects.Player.Movement.States
{
    [Serializable]
    public class PlayerJumpState : MovementStateBase
    {
        [SerializeField] private float jumpMult = 2;
        [SerializeField] private float jumpHeight = 2;

        public override void Starter(SimpleFPSController simpleFPSController)
        {
            base.Starter(simpleFPSController);
            jumpMult = Mathf.Sqrt(jumpHeight * -2f * simpleFPSController.playerMovementController.gravity);
        }

        public override void Enter()
        {
            Update();
        }
        public override void Update()
        {
            if (movementController.isGrounded) {
                movementController.yVelocity += jumpMult;
                Debug.Log("yes");
            }
        }
    }
}
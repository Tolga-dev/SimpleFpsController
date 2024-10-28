using System;
using GameObjects.Player.Input;
using GameObjects.Player.Movement.States;
using UnityEngine;

namespace GameObjects.Player.Movement
{
    [Serializable]
    public class PlayerMovementController
    {
        private SimpleFPSController simpleFPSController;
        private PlayerInputController input;

        public MovementStateBase movementStateBase;
        public PlayerClimbingState playerClimbingState;
        public PlayerProneState playerProneState;
        public PlayerSlidingState playerSlidingState;
        public PlayerJumpState playerJumpState;
        public PlayerCrouchState playerCrouchState;
        public PlayerSprintState playerSprintState;
        
        public float originalHeight;
        public bool isGrounded;
        
        public CharacterController charController;

        // input 
        public Vector3 move;
        // high velocity
        public float yVelocity;

        // slope 
        private float originalSlopeLimit;
        [SerializeField, Range(0f, 90f)] private float jumpSlopeLimit;
        
        // transform
        public Transform transform;
        
        // speed 
        [SerializeField] private float speed = 10;
        [SerializeField] public float gravity = -20f;

        public void Starter(SimpleFPSController fpsController)
        {
            originalSlopeLimit = charController.slopeLimit;
            originalHeight = charController.height;

            simpleFPSController = fpsController;
            input = fpsController.input;
            
            playerSprintState.Starter(fpsController);
            playerCrouchState.Starter(fpsController);
            playerJumpState.Starter(fpsController);
            playerSlidingState.Starter(fpsController);
            playerProneState.Starter(fpsController);
            playerClimbingState.Starter(fpsController);
        }
        
        public void Update()
        {
            OnAir();
            
            PlayerMovementDirection();
            
            ApplyMovement();
        }
        private void ApplyMovement()
        {
            move.y = yVelocity * Time.deltaTime;
            charController.Move(move);
        }
        
        private void PlayerMovementDirection()
        {
            move = (transform.right * input.x + transform.forward * input.z).normalized;
            move *= (speed * Time.deltaTime);

            CheckForActionStates();
        }

        private void CheckForActionStates() // u can use better way
        {
            if (input.isProne)
            {
                SwitchState(playerProneState);
            }
            else if (input.isSliding)
            {
                SwitchState(playerSlidingState);
            }
            else if (input.isClimbing)
            {
                SwitchState(playerClimbingState);
            }
            
            if (input.isRunning)
            {
                SwitchState(playerSprintState);
            }
            if (input.isCrouching)
            {
                SwitchState(playerCrouchState);
            }
            if (input.isJumping)
            {
                SwitchState(playerJumpState);
            }
        }

        private void OnAir()
        {
            isGrounded = charController.isGrounded;
            
            if (isGrounded || charController.collisionFlags == CollisionFlags.Above) yVelocity = -0.1f;

            if (isGrounded) {
                charController.slopeLimit = originalSlopeLimit;
            }
            else {
                charController.slopeLimit = jumpSlopeLimit;
            }
            
            if (input.isClimbing == false)
            {
                yVelocity += gravity * Time.deltaTime;
            }
        }

        public void SwitchState(MovementStateBase stateBase)
        {
            if (stateBase == movementStateBase)
            {
                stateBase.Update();
                return;
            }
            
            movementStateBase?.Exit();
            movementStateBase = stateBase;
            movementStateBase.Enter();
        }

  
    }
}
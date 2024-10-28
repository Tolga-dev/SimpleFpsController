using GameObjects.Player.Input;
using UnityEngine;

namespace GameObjects.Player.Movement.States
{
    public class MovementStateBase
    {
        protected CharacterController charController;
        
        protected PlayerMovementController movementController;
        protected PlayerInputController inputController;

        public virtual void Starter(SimpleFPSController simpleFPSController)
        {
            movementController = simpleFPSController.playerMovementController;
            inputController = simpleFPSController.input;
            charController = simpleFPSController.playerMovementController.charController;
        }
        public virtual void Enter()
        {
            
        }
        
        public virtual void Update()
        {
            
        }

        public virtual void Exit()
        {
            
        }
    }
}
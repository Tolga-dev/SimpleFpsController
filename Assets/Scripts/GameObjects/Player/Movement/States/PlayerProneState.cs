using System;

namespace GameObjects.Player.Movement.States
{
    [Serializable]
    public class PlayerProneState : MovementStateBase
    {
        // Prone settings
        public float proneSpeedMultiplier = 0.5f;
        public float proneHeight = 0.3f;

        public override void Update()
        {
            if (inputController.isProne)
            {
                
                charController.height = proneHeight;
                movementController.move *= proneSpeedMultiplier;
            }
            else
            {
                charController.height = movementController.originalHeight;
            }
            
        }
        
    }
}       
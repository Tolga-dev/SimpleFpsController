using System;
using UnityEngine;

namespace GameObjects.Player.Movement.States
{
    [Serializable]
    public class PlayerSlidingState : MovementStateBase
    {
        public float slideSpeedMultiplier = 1.5f;
        public float slideDuration = 0.5f;
        public float slideCooldown = 1f;
        private float slideTimer = 0;
        private bool isSliding = false;
        private float originalHeight;
        public float slideHeight = 0.5f;
        
        public override void Update()
        {
            var z = inputController.z;
            
            if (charController.isGrounded && z > 0 && slideTimer <= 0)
            {
                StartSlide();
            }
            
            if (isSliding)
            {
                movementController.move *= slideSpeedMultiplier;
                slideTimer -= Time.deltaTime;
                if (slideTimer <= 0)
                {
                    StopSlide();
                }
            }

        }
        
        private void StartSlide()
        {
            isSliding = true;
            slideTimer = slideDuration;
            charController.height = slideHeight;
        }

        private void StopSlide()
        {
            isSliding = false;
            slideTimer = -slideCooldown; // Set cooldown time
            charController.height = originalHeight;
        }
        
        
    }
}
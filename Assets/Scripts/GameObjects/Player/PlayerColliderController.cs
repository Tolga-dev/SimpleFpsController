using System;
using GameObjects.HealthPack;
using GameObjects.Player.Input;
using GameObjects.Player.Movement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameObjects.Player
{
    [Serializable]
    public class PlayerColliderController
    {
        private SimpleFPSController simpleFPSController;
        private PlayerInputController input;
        private PlayerMovementController playerMovementController;

        public void Starter(SimpleFPSController fpsController)
        {
            simpleFPSController = fpsController;
            input = simpleFPSController.input;
            playerMovementController = simpleFPSController.playerMovementController;
        }
        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ladder"))
            {
                input.isClimbing = true;
                playerMovementController.yVelocity = 0f;  // Reset yVelocity when starting to climb
            }
            else if (other.CompareTag("Health"))
            {
                var playerHealth = other.GetComponent<HealthPackBase>(); // Assuming your health script is named PlayerHealth
                
                if (playerHealth != null)
                {
                    playerHealth.PickupHealth(simpleFPSController);
                    Object.Destroy(playerHealth.gameObject);  // Destroy the health pack after use
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Ladder"))
            {
                input.isClimbing = false;
            }
        }
    }
}
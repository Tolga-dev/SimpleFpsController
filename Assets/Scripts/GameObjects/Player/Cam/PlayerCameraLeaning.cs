using System;
using GameObjects.Player.Input;
using UnityEngine;

namespace GameObjects.Player.Cam
{
    [Serializable]
    public class PlayerCameraLeaning
    {
        public float leanAngle = 15f;   // Angle for leaning
        public float leanSpeed = 5f;    // Speed at which the player leans
        private float currentLean = 0f;
        private PlayerCameraController playerCameraController;
        private PlayerInputController inputController;
        
        public void Starter(SimpleFPSController simpleFPSController, PlayerCameraController cameraController)
        {
            playerCameraController = cameraController;
            inputController = simpleFPSController.input;
        }

        public void HandleLeaning()
        {
            float targetLean = 0f;
    
            if (inputController.isLeaningLeft)
            {
                targetLean = leanAngle; // Lean left
            }
            else if (inputController.isLeaningRight)
            {
                targetLean = -leanAngle; // Lean right
            }

            currentLean = Mathf.Lerp(currentLean, targetLean, Time.deltaTime * leanSpeed);

            var playerCamera = playerCameraController.playerCamera;
            var transform1 = playerCamera.transform;
            playerCamera.transform.localRotation = Quaternion.Euler(transform1.localRotation.eulerAngles.x,
                transform1.localRotation.eulerAngles.y, currentLean);
        }

    }
}
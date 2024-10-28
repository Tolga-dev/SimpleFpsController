using System;
using GameObjects.Player.Input;
using UnityEngine;

namespace GameObjects.Player.Cam
{
    [Serializable]
    public class PlayerCameraController 
    {
        private SimpleFPSController simpleFPSController;
        private PlayerCameraLeaning playerCameraLeaning = new PlayerCameraLeaning();
        private PlayerInputController inputController;
        
        public Camera playerCamera;
        public float maxLookAngle = 50f;
        
        [SerializeField] 
        private float mouseSensitivity;
        private float xRotation = 0f;

        public void Starter(SimpleFPSController fpsController)
        {
            simpleFPSController = fpsController;
            inputController = fpsController.input;

            playerCameraLeaning.Starter(fpsController, this);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
        }
        public void LateUpdate()
        {
            float mouseX = inputController.mouseX * mouseSensitivity * Time.deltaTime;
            float mouseY = inputController.mouseY * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);

            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            simpleFPSController.transform.Rotate(Vector3.up * mouseX);
            
            playerCameraLeaning.HandleLeaning();
        }
        
    }
}
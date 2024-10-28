using GameObjects.HealthPack;
using GameObjects.Player.Cam;
using GameObjects.Player.Input;
using GameObjects.Player.Movement;
using UnityEngine;

namespace GameObjects.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class SimpleFPSController : MonoBehaviour
    {
        public PlayerInputController input;
        public PlayerCameraController cameraController;
        public PlayerMovementController playerMovementController;
        public PlayerHealthController playerHealthController;
        public PlayerColliderController playerColliderController;
        
        private void Start() {
            playerMovementController.Starter(this);
            playerColliderController.Starter(this);
            playerHealthController.Starter();
            cameraController.Starter(this);
        }
        private void Update() {
            input.Update();
            playerMovementController.Update();
            playerHealthController.RegenerateHealth();
        }

        private void LateUpdate() {
            cameraController.LateUpdate();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            playerColliderController.OnTriggerEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
            playerColliderController.OnTriggerExit(other);
        } 
    }
}

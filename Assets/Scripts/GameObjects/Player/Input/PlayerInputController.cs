using System;
using UnityEngine;

namespace GameObjects.Player.Input
{
    [Serializable]
    public class PlayerInputController
    {
        public bool isJumping;
        public bool isRunning;
        public bool isCrouching;
        public bool isSliding;
        public bool isProne;
        public bool isLeaningLeft;
        public bool isLeaningRight;
        public bool isClimbing;

        public float mouseX;
        public float mouseY;

        public float x;
        public float z;
        
        public KeyCode jumpKey = KeyCode.Space;
        public KeyCode runKey = KeyCode.LeftShift;
        public KeyCode crouchKey = KeyCode.C;
        public KeyCode slideKey = KeyCode.LeftControl;
        public KeyCode proneKey = KeyCode.Z;
        public KeyCode leanLeftKey = KeyCode.Q;
        public KeyCode leanRightKey = KeyCode.E;
        
        public void Update()
        {
            isJumping = UnityEngine.Input.GetKey(jumpKey);
            isRunning = UnityEngine.Input.GetKey(runKey);
            isCrouching = UnityEngine.Input.GetKey(crouchKey);
            isSliding = UnityEngine.Input.GetKeyDown(slideKey);
            isProne = UnityEngine.Input.GetKey(proneKey);
            isLeaningLeft = UnityEngine.Input.GetKey(leanLeftKey);
            isLeaningRight = UnityEngine.Input.GetKey(leanRightKey);
            
            mouseX = UnityEngine.Input.GetAxis("Mouse X");
            mouseY = UnityEngine.Input.GetAxis("Mouse Y");
            
            x = UnityEngine.Input.GetAxisRaw("Horizontal");
            z = UnityEngine.Input.GetAxisRaw("Vertical");
        }
    }
}
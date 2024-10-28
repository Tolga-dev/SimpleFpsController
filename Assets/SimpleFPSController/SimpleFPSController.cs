using GameObjects.HealthPack;
using UnityEngine;

namespace SimpleFPSController
{
    [RequireComponent(typeof(CharacterController))]
    public class SimpleFPSController : MonoBehaviour {

        [SerializeField] private float speed = 10;
        [SerializeField] private float gravity = -20f;
        [SerializeField] private float jumpHeight = 2;
        [SerializeField] private float runMultiplier = 2;
        [SerializeField, Range(0f, 90f)] private float jumpSlopeLimit;

        private CharacterController charController;
        private float jumpMult;
        private float yVelocity;
        private float originalSlopeLimit;
        private bool isGrounded;

        // Mouse look
        private float xRotation = 0f;
        [SerializeField] private float mouseSensitivity;
        public Camera playerCamera;
        public float maxLookAngle = 50f;
        
        
        public float slideSpeedMultiplier = 1.5f;
        public float slideDuration = 0.5f;
        public float slideCooldown = 1f;
        private float slideTimer = 0;
        private bool isSliding = false;
        private float originalHeight;
        public float slideHeight = 0.5f;

        // Prone settings
        public float proneSpeedMultiplier = 0.5f;
        public float proneHeight = 0.3f;
        private bool isProne = false;
        
        public float leanAngle = 15f;   // Angle for leaning
        public float leanSpeed = 5f;    // Speed at which the player leans
        private float currentLean = 0f;
        public float climbSpeed = 3f;
        public bool isClimbing = false;
        
        // health
        public float maxHealth = 100f;
        public float currentHealth;
        public float regenRate = 5f;        // Health regenerated per second
        public float regenDelay = 3f;       // Delay after taking damage before regen starts
        private float regenTimer = 0f;      // Tracks time since last damage

        private void Start() {
            charController = GetComponent<CharacterController>();

            originalSlopeLimit = charController.slopeLimit;
            jumpMult = Mathf.Sqrt(jumpHeight * -2f * gravity);
            currentHealth = maxHealth;      // Set initial health
        
            // Hide mouse and lock to screen center
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            originalHeight = charController.height;
        }

        void Update() {
        
            if (Input.GetKeyDown(KeyCode.Equals)) mouseSensitivity = Mathf.Clamp(mouseSensitivity + 20, 20f, 1000f);
            if (Input.GetKeyDown(KeyCode.Minus)) mouseSensitivity = Mathf.Clamp(mouseSensitivity - 20, 20f, 1000f);

            PlayerMove();
            RegenerateHealth();
        }

        void LateUpdate() {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);

            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
            HandleLeaning();
        }
     

        private void PlayerMove() {
            isGrounded = charController.isGrounded;
            if (charController.isGrounded || charController.collisionFlags == CollisionFlags.Above) yVelocity = -0.1f;

            if (charController.isGrounded) {
                charController.slopeLimit = originalSlopeLimit;
            }
            else {
                charController.slopeLimit = jumpSlopeLimit;
            }
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            Vector3 move = (transform.right * x + transform.forward * z).normalized;
            move *= (speed * Time.deltaTime);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                move *= runMultiplier;
            }
            if (Input.GetKey(KeyCode.C))
            {
                move /= runMultiplier;
            }
            if (Input.GetButtonDown("Jump") && charController.isGrounded) {
                yVelocity += jumpMult;
            }
            
            if (Input.GetKeyDown(KeyCode.LeftControl) && charController.isGrounded && z > 0 && slideTimer <= 0)
            {
                StartSlide();
            }
            if (isSliding)
            {
                move *= slideSpeedMultiplier;
                slideTimer -= Time.deltaTime;
                if (slideTimer <= 0)
                {
                    StopSlide();
                }
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                ToggleProne();
            }
            if (isProne)
            {
                move *= proneSpeedMultiplier;
            }
            
            if (isClimbing)
            {
                yVelocity += z * climbSpeed * Time.deltaTime;
                move.z = 0;
                
            }
            else
            {
                yVelocity += gravity * Time.deltaTime;
            }

            move.y = yVelocity * Time.deltaTime;

            charController.Move(move);
        }

        private void ToggleProne()
        {
            if (isProne)
            {
                isProne = false;
                charController.height = originalHeight;
            }
            else
            {
                isProne = true;
                charController.height = proneHeight;
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
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ladder"))
            {
                isClimbing = true;
                yVelocity = 0f;  // Reset yVelocity when starting to climb
            }
            else if (other.CompareTag("Health"))
            {
                var playerHealth = other.GetComponent<HealthPackBase>(); // Assuming your health script is named PlayerHealth
                
                if (playerHealth != null)
                {
                    playerHealth.PickupHealth(this);
                    Destroy(playerHealth.gameObject);  // Destroy the health pack after use
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Stop climbing when leaving the ladder
            if (other.CompareTag("Ladder"))
            {
                isClimbing = false;
            }
        }
        
        private void HandleLeaning()
        {
            float targetLean = 0f;
    
            // Check for lean input
            if (Input.GetKey(KeyCode.Q))
            {
                targetLean = leanAngle; // Lean left
            }
            else if (Input.GetKey(KeyCode.E))
            {
                targetLean = -leanAngle; // Lean right
            }

            // Smoothly interpolate to the target lean angle
            currentLean = Mathf.Lerp(currentLean, targetLean, Time.deltaTime * leanSpeed);
    
            // Apply the lean to the camera by rotating it around the Z-axis
            var transform1 = playerCamera.transform;
            
            playerCamera.transform.localRotation = Quaternion.Euler(transform1.localRotation.eulerAngles.x,
                transform1.localRotation.eulerAngles.y, currentLean);
        }
        
        
        private void RegenerateHealth()
        { 
            if (currentHealth < maxHealth && regenTimer >= regenDelay)
            {
                currentHealth += regenRate * Time.deltaTime;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't exceed max
            }
            else
            {
                regenTimer += Time.deltaTime;  // Increase timer if delay hasn't passed
            }
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            regenTimer = 0f;  // Reset timer when taking damage
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            // Implement death logic here
            Debug.Log("Player has died!");
        }
    }
}

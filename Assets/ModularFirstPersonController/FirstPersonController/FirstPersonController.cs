using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ModularFirstPersonController.FirstPersonController
{
    public class FirstPersonController : MonoBehaviour
    {

        #region Camera Movement Variables

        public Camera playerCamera;

        public float fov = 60f;
        public bool invertCamera = false;
        public bool cameraCanMove = true;
        public float mouseSensitivity = 2f;
        public float maxLookAngle = 50f;

        // Crosshair
        public bool lockCursor = true;
        public bool crosshair = true;
        public Sprite crosshairImage;
        public Color crosshairColor = Color.white;

        // Internal Variables
        private float yaw = 0.0f;
        private float pitch = 0.0f;
        private Image crosshairObject;

        #region Camera Zoom Variables

        public bool enableZoom = true;
        public bool holdToZoom = false;
        public KeyCode zoomKey = KeyCode.Mouse1;
        public float zoomFOV = 30f;
        public float zoomStepTime = 5f;

        // Internal Variables
        private bool isZoomed = false;

        #endregion

        #endregion

        #region Movement Variables

        public bool playerCanMove = true;
        public float walkSpeed = 5f;
        public float maxVelocityChange = 10f;

        // Internal Variables
        private bool isWalking = false;

        #region Sprint

        public bool enableSprint = true;
        public bool unlimitedSprint = false;
        public KeyCode sprintKey = KeyCode.LeftShift;
        public float sprintSpeed = 7f;
        public float sprintDuration = 5f;
        public float sprintCooldown = .5f;
        public float sprintFOV = 80f;
        public float sprintFOVStepTime = 10f;

        // Sprint Bar
        public bool useSprintBar = true;
        public bool hideBarWhenFull = true;
        public Image sprintBarBG;
        public Image sprintBar;
        public float sprintBarWidthPercent = .3f;
        public float sprintBarHeightPercent = .015f;

        // Internal Variables
        private CanvasGroup sprintBarCG;
        private bool isSprinting = false;
        private float sprintRemaining;
        private float sprintBarWidth;
        private float sprintBarHeight;
        private bool isSprintCooldown = false;
        private float sprintCooldownReset;

        #endregion

        #region Jump

        public bool enableJump = true;
        public KeyCode jumpKey = KeyCode.Space;
        public float jumpPower = 5f;

        // Internal Variables
        public bool isGrounded = false;

        #endregion

        #region Crouch

        public bool enableCrouch = true;
        public bool holdToCrouch = true;
        public KeyCode crouchKey = KeyCode.LeftControl;
        public float crouchHeight = .75f;
        public float speedReduction = .5f;

        // Internal Variables
        private bool isCrouched = false;
        private Vector3 originalScale;

        #endregion

        #endregion

        #region Head Bob

        public bool enableHeadBob = true;
        public Transform joint;
        public float bobSpeed = 10f;
        public Vector3 bobAmount = new Vector3(.15f, .05f, 0f);

        // Internal Variables
        private Vector3 jointOriginalPos;
        private float timer = 0;

        #endregion


        // Required components
        public CharacterController characterController;
        public float gravity = 9.81f;
        public Transform foot;
        public float distance;
        public float velocityBooster;

        private void Awake()
        {
            crosshairObject = GetComponentInChildren<Image>();

            // Set internal variables
            playerCamera.fieldOfView = fov;
            originalScale = transform.localScale;
            jointOriginalPos = joint.localPosition;

            if (!unlimitedSprint)
            {
                sprintRemaining = sprintDuration;
                sprintCooldownReset = sprintCooldown;
            }
        }

        void Start()
        {
            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            if (crosshair)
            {
                crosshairObject.sprite = crosshairImage;
                crosshairObject.color = crosshairColor;
            }
            else
            {
                crosshairObject.gameObject.SetActive(false);
            }

            #region Sprint Bar

            sprintBarCG = GetComponentInChildren<CanvasGroup>();

            if (useSprintBar)
            {
                sprintBarBG.gameObject.SetActive(true);
                sprintBar.gameObject.SetActive(true);

                float screenWidth = Screen.width;
                float screenHeight = Screen.height;

                sprintBarWidth = screenWidth * sprintBarWidthPercent;
                sprintBarHeight = screenHeight * sprintBarHeightPercent;

                sprintBarBG.rectTransform.sizeDelta = new Vector3(sprintBarWidth, sprintBarHeight, 0f);
                sprintBar.rectTransform.sizeDelta = new Vector3(sprintBarWidth - 2, sprintBarHeight - 2, 0f);

                if (hideBarWhenFull)
                {
                    sprintBarCG.alpha = 0;
                }
            }
            else
            {
                sprintBarBG.gameObject.SetActive(false);
                sprintBar.gameObject.SetActive(false);
            }

            #endregion
        }

        float camRotation;

        private void Update()
        {
          //  RotationCamPlayer();

            if (enableZoom)
            {
                // Changes isZoomed when key is pressed
                // Behavior for toogle zoom
                if (Input.GetKeyDown(zoomKey) && !holdToZoom && !isSprinting)
                {
                    if (!isZoomed)
                    {
                        isZoomed = true;
                    }
                    else
                    {
                        isZoomed = false;
                    }
                }

                // Changes isZoomed when key is pressed
                // Behavior for hold to zoom
                if (holdToZoom && !isSprinting)
                {
                    if (Input.GetKeyDown(zoomKey))
                    {
                        isZoomed = true;
                    }
                    else if (Input.GetKeyUp(zoomKey))
                    {
                        isZoomed = false;
                    }
                }

                // Lerps camera.fieldOfView to allow for a smooth transistion
                if (isZoomed)
                {
                    playerCamera.fieldOfView =
                        Mathf.Lerp(playerCamera.fieldOfView, zoomFOV, zoomStepTime * Time.deltaTime);
                }
                else if (!isZoomed && !isSprinting)
                {
                    playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, zoomStepTime * Time.deltaTime);
                }
            }

            
            if (isSprinting)
            {
                isZoomed = false;
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, sprintFOV,
                    sprintFOVStepTime * Time.deltaTime);

                // Drain sprint remaining while sprinting
                if (!unlimitedSprint)
                {
                    sprintRemaining -= 1 * Time.deltaTime;
                    if (sprintRemaining <= 0)
                    {
                        isSprinting = false;
                        isSprintCooldown = true;
                    }
                }
            }
            else
            {
                // Regain sprint while not sprinting
                sprintRemaining = Mathf.Clamp(sprintRemaining += 1 * Time.deltaTime, 0, sprintDuration);
            }
            

            // Handles sprint cooldown 
            // When sprint remaining == 0 stops sprint ability until hitting cooldown
            
            if (isSprintCooldown)
            {
                sprintCooldown -= 1 * Time.deltaTime;
                if (sprintCooldown <= 0)
                {
                    isSprintCooldown = false;
                }
            }
            else
            {
                sprintCooldown = sprintCooldownReset;
            }

            
            // Handles sprintBar 
            if (useSprintBar && !unlimitedSprint)
            {
                float sprintRemainingPercent = sprintRemaining / sprintDuration;
                sprintBar.transform.localScale = new Vector3(sprintRemainingPercent, 1f, 1f);
            }
            

            // Gets input and calls jump method
            if (enableJump && Input.GetKeyDown(jumpKey) && isGrounded)
            {
                Jump();
            }
            CheckGround();

            if (enableCrouch)
            {
                if (Input.GetKeyDown(crouchKey) && !holdToCrouch)
                {
                    Crouch();
                }

                if (Input.GetKeyDown(crouchKey) && holdToCrouch)
                {
                    isCrouched = false;
                    Crouch();
                }
                else if (Input.GetKeyUp(crouchKey) && holdToCrouch)
                {
                    isCrouched = true;
                    Crouch();
                }
            }


            if (enableHeadBob)
            {
                HeadBob();
            }
        }

        
        private void RotationCamPlayer()
        {
            if (cameraCanMove)
            {
                yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;

                if (!invertCamera)
                {
                    pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
                }
                else
                {
                    // Inverted Y
                    pitch += mouseSensitivity * Input.GetAxis("Mouse Y");
                }

                // Clamp pitch between lookAngle
                pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

                transform.localEulerAngles = new Vector3(0, yaw, 0);
                playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
            }
        }
        

        private void FixedUpdate()
        {
            Vector3 targetVelocity = GetMovementVector();


            UpdateWalkingState(targetVelocity);
            ApplyGravity(ref targetVelocity);
            
            if (CanSprint())
            {
                ApplySprint(ref targetVelocity);
            }
            else
            {
                ApplyWalk(ref targetVelocity);
            }

            
            characterController.Move(targetVelocity * Time.deltaTime);
        }

        private Vector3 GetMovementVector()
        {
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            return transform.TransformDirection(targetVelocity); // Convert to world space
        }

        private void UpdateWalkingState(Vector3 targetVelocity)
        {
            isWalking = (targetVelocity.x != 0 || targetVelocity.z != 0) && isGrounded;
        }

        private bool CanSprint()
        {
            return Input.GetKey(sprintKey) && sprintRemaining > 0f && !isSprintCooldown;
        }

        private void ApplySprint(ref Vector3 targetVelocity)
        {
            targetVelocity *= sprintSpeed;
            isSprinting = true;

            if (isCrouched)
            {
                Crouch();
            }

            if (hideBarWhenFull && !unlimitedSprint)
            {
                sprintBarCG.alpha += 5 * Time.deltaTime;
            }
        }

        private void ApplyWalk(ref Vector3 targetVelocity)
        {
            targetVelocity *= walkSpeed;
            isSprinting = false;

            if (hideBarWhenFull && sprintRemaining == sprintDuration)
            {
                sprintBarCG.alpha -= 3 * Time.deltaTime;
            }
        }

        private void ApplyGravity(ref Vector3 targetVelocity)
        {
            if (!characterController.isGrounded)
            {
                targetVelocity.y = velocityBooster * gravity * Time.deltaTime;
            }
        }

        // Sets isGrounded based on a raycast sent straigth down from the player object
        private void CheckGround()
        {
            var origin = foot.transform.position;
            var direction = Vector3.down;

            if (Physics.Raycast(origin, direction, out var hit, distance))
            {
                Debug.DrawRay(origin, direction * distance, Color.green);
                isGrounded = true;
            }
            else
            {
                Debug.DrawRay(origin, direction * distance, Color.red);
                isGrounded = false;
            }
        }


        private void Jump()
        {
            if (isGrounded)
            {
                characterController.Move(new Vector3(0f, jumpPower, 0f));
                isGrounded = false;
            }

            // When crouched and using toggle system, will uncrouch for a jump
            if (isCrouched && !holdToCrouch)
            {
                Crouch();
            }
        }

        private void Crouch()
        {
            if (isCrouched)
            {
                transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
                walkSpeed /= speedReduction;

                isCrouched = false;
            }
            else
            {
                transform.localScale = new Vector3(originalScale.x, crouchHeight, originalScale.z);
                walkSpeed *= speedReduction;

                isCrouched = true;
            }
        }

        private void HeadBob()
        {
            if (isWalking)
            {
                // Calculates HeadBob speed during sprint
                if (isSprinting)
                {
                    timer += Time.deltaTime * (bobSpeed + sprintSpeed);
                }
                // Calculates HeadBob speed during crouched movement
                else if (isCrouched)
                {
                    timer += Time.deltaTime * (bobSpeed * speedReduction);
                }
                // Calculates HeadBob speed during walking
                else
                {
                    timer += Time.deltaTime * bobSpeed;
                }

                // Applies HeadBob movement
                joint.localPosition = new Vector3(jointOriginalPos.x + Mathf.Sin(timer) * bobAmount.x,
                    jointOriginalPos.y + Mathf.Sin(timer) * bobAmount.y,
                    jointOriginalPos.z + Mathf.Sin(timer) * bobAmount.z);
            }
            else
            {
                // Resets when play stops moving
                timer = 0;
                joint.localPosition = new Vector3(
                    Mathf.Lerp(joint.localPosition.x, jointOriginalPos.x, Time.deltaTime * bobSpeed),
                    Mathf.Lerp(joint.localPosition.y, jointOriginalPos.y, Time.deltaTime * bobSpeed),
                    Mathf.Lerp(joint.localPosition.z, jointOriginalPos.z, Time.deltaTime * bobSpeed));
            }
        }
    }

}
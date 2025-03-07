using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Player Control
        [Header("Movement settings")] 
        [SerializeField][Range(0f,15f)]private float moveSpeed = 5f;
        [SerializeField][Range(0f,20f)]private float jumpForce = 5f;
        [SerializeField][Range(0f,15f)]private float mouseSensitivity = 2f;
   
        [SerializeField][Range(0f, 25f)] private float controllerSensitivityX = 15f;
        [SerializeField][Range(0f, 25f)] private float controllerSensitivityY = 15f;
    #endregion
    #region ControlScheme checks

        private bool isMouse;
        private bool isController;
        
    #endregion
    #region Refs
        [Header("References")] 
        public Transform cameraTransform;
        private Rigidbody rb;
        private Vector2 moveInput;
        private Vector2 lookInput;
        private bool jumpInput;
        private PlayerInput playerInput;
        [SerializeField]private bool isGrounded;
        public LayerMask groundLayer;
        public Transform groundCheck;
    #endregion
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        //when player moves
        //note:listen for each action when performed reads vector and when canceled vector 2 is zero, must stop movement
        playerInput.actions["Move"].performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Move"].canceled += ctx => moveInput = Vector2.zero;
        //player look
        playerInput.actions["Look"].performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Look"].canceled += ctx => lookInput = Vector2.zero;
        //player jump
        playerInput.actions["Jump"].performed += ctx => jumpInput = true;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
        HandleLook(); 
    }
    
    //use lateUpdate() - to process the camera movement after player has moved
    private void LateUpdate()
    { 

    }

    [ContextMenu("HandleMovement")]
    private void HandleMovement()
    {
        Vector3 move = transform.right  * moveInput.x + transform.forward * moveInput.y;
        move *= moveSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }

    private void HandleJump()
    {
        //since no CC component is used, we create a isGrounded that tells when player
        //is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        if (jumpInput )
        {                                      //Apply instant force
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);// 2 arguments, direction + forcemode type
        }

        jumpInput = false;
    }
    
    private void HandleLook()
    {
      
        float lookX = lookInput.x;
        float lookY = lookInput.y;

// Apply different sensitivities for mouse and controller
        float sensitivityX = isMouse ? mouseSensitivity : controllerSensitivityX;
        float sensitivityY = isMouse ? mouseSensitivity : controllerSensitivityY;

// Apply deadzone to controller input for smoother handling of small stick movements
        float controllerDeadzone = 0.2f;  // Set this to your preferred deadzone threshold
        if (isController)
        {
            if (Mathf.Abs(lookX) < controllerDeadzone) lookX = 0;
            if (Mathf.Abs(lookY) < controllerDeadzone) lookY = 0;

            // Increase sensitivity for X and Y axis separately
            lookX *= sensitivityX; // X-axis sensitivity for controller
            lookY *= sensitivityY; // Y-axis sensitivity for controller
        }
        else
        {
            lookX *= sensitivityX; // Mouse sensitivity remains as is
            lookY *= sensitivityY;
        }

// Rotate the player horizontally (Y-axis)
        transform.Rotate(Vector3.up * lookX);

// Clamp vertical camera rotation between -80 and 80 degrees
        float currentXRotation = cameraTransform.localEulerAngles.x;
        float newRotationX = currentXRotation - lookY;

        if (newRotationX > 180)
        {
            newRotationX -= 360;
        }

        newRotationX = Mathf.Clamp(newRotationX, -80f, 80f);

// Smooth the vertical rotation (camera pitch) movement
        float smoothSpeed = 10f;
        float smoothedRotationX = Mathf.LerpAngle(cameraTransform.localEulerAngles.x, newRotationX, Time.deltaTime * smoothSpeed);

        cameraTransform.localEulerAngles = new Vector3(smoothedRotationX, cameraTransform.localEulerAngles.y, 0);
    }





}
 
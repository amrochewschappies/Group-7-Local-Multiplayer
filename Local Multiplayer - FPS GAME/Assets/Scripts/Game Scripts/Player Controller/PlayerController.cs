using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Player Control

    [Header("Movement settings")]
    //move
    [SerializeField]
    [Range(0f, 15f)]
    private float moveSpeed = 5f;

    //jump
    [SerializeField] [Range(0f, 20f)] private float jumpForce = 5f;
    private float gravityMultiplier = 3f;
    private float lowJumpMultiplier = 4f;
    private float coyoteTime = 0.2f;
    private float jumpBufferTime = 0.2f;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    //sensitivity
    [SerializeField] [Range(0f, 15f)] private float mouseSensitivity = 2f;
    [SerializeField] [Range(0f, 25f)] private float controllerSensitivityX = 15f;
    [SerializeField] [Range(0f, 25f)] private float controllerSensitivityY = 15f;

    #endregion

    #region ControlScheme checks

    private bool isMouse;
    private bool isController;

    #endregion

    #region Refs

    [Header("References")] public Transform cameraTransform;
    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool jumpInput;
    private PlayerInput playerInput;
    [SerializeField] private bool isGrounded;
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
        ApplyGravity();
    }

    //use lateUpdate() - to process the camera movement after player has moved
    private void LateUpdate()
    {

    }

    [ContextMenu("HandleMovement")]
    private void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move *= moveSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }



    private void HandleLook()
    {

        float lookX = lookInput.x;
        float lookY = lookInput.y;
        
        float sensitivityX = isMouse ? mouseSensitivity : controllerSensitivityX;
        float sensitivityY = isMouse ? mouseSensitivity : controllerSensitivityY;
        
        float controllerDeadzone = 0.2f; 
        if (isController)
        {
            if (Mathf.Abs(lookX) < controllerDeadzone) lookX = 0;
            if (Mathf.Abs(lookY) < controllerDeadzone) lookY = 0;
            
            lookX *= sensitivityX;
            lookY *= sensitivityY; 
        }
        else
        {
            lookX *= sensitivityX; 
            lookY *= sensitivityY;
        }
        
        transform.Rotate(Vector3.up * lookX);
        
        float currentXRotation = cameraTransform.localEulerAngles.x;
        float newRotationX = currentXRotation - lookY;

        if (newRotationX > 180)
        {
            newRotationX -= 360;
        }

        newRotationX = Mathf.Clamp(newRotationX, -80f, 80f);
        
        float smoothSpeed = 10f;
        float smoothedRotationX =
            Mathf.LerpAngle(cameraTransform.localEulerAngles.x, newRotationX, Time.deltaTime * smoothSpeed);

        cameraTransform.localEulerAngles = new Vector3(smoothedRotationX, cameraTransform.localEulerAngles.y, 0);
    }

    private void HandleJump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime; 
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (jumpInput)
        {
            jumpBufferCounter = jumpBufferTime; 
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            jumpBufferCounter = 0; 
        }

        jumpInput = false; 
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            if (rb.linearVelocity.y > 0 && !jumpInput) 
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime; // stronger grav pull when releasing jump early
            }
            else
            {
             
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (gravityMultiplier - 1) * Time.deltaTime; // apply higher gravity scale if player ever falls faster
            }
        }

    }
}
 
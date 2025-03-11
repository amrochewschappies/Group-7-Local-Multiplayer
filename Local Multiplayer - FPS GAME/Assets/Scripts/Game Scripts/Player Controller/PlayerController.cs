using System;
using System.Collections;
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
    private float coyoteTime = 0.1f;
    private float jumpBufferTime = 0.2f;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    //sensitivity
    [SerializeField] [Range(0f, 15f)] private float mouseSensitivity = 2f;
    [SerializeField] [Range(0f, 25f)] private float controllerSensitivityX = 15f;
    [SerializeField] [Range(0f, 25f)] private float controllerSensitivityY = 15f;
    
    //deadzone
    [SerializeField][Range(0f, 1f)] private float controllerDeadzone = 0.5f;

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
    private Vector3 moveTile;
    private bool jumpInput;
    private PlayerInput playerInput;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool wasInAir = false;
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
        
        
        //move tile
        //playerInput.actions["MoveTile"].performed += ctx => moveTile = ctx.ReadValue<Vector3>();
        //playerInput.actions["MoveTile"].canceled += ctx => moveTile = Vector3.zero;

    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
        HandleLook();
        MoveTile();
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



    private float xRotation = 0f; // Stores current vertical rotation (prevents flipping)

    private void HandleLook()
    {
        float lookX = lookInput.x;
        float lookY = lookInput.y;

        if (isMouse)
        {
            float sensitivityX = mouseSensitivity * 0.5f;
            float sensitivityY = mouseSensitivity * 0.5f;

            lookX *= sensitivityX;
            lookY *= sensitivityY;

         
            transform.Rotate(Vector3.up * lookX);

     
            xRotation -= lookY;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
            
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
        else
        {
            float sensitivityX = controllerSensitivityX;
            float sensitivityY = controllerSensitivityY;

    
            if (Mathf.Abs(lookX) < controllerDeadzone) lookX = 0;
            if (Mathf.Abs(lookY) < controllerDeadzone) lookY = 0;

            lookX *= sensitivityX;
            lookY *= sensitivityY;
            
            Quaternion horizontalRotation = Quaternion.Euler(0f, lookX, 0f);
            transform.rotation *= horizontalRotation;

            float newRotationX = cameraTransform.localEulerAngles.x - lookY;
            if (newRotationX > 180) newRotationX -= 360;
            newRotationX = Mathf.Clamp(newRotationX, -80f, 80f);

            Quaternion verticalRotation = Quaternion.Euler(newRotationX, 0f, 0f);
            cameraTransform.localRotation = Quaternion.Slerp(cameraTransform.localRotation, verticalRotation, Time.deltaTime * 10f);
        }
    }



    private void HandleJump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded)
        {
            if (wasInAir) 
            {
                StartCoroutine(TriggerRumble(0.5f, 0.7f, 0.2f)); 
            }
            coyoteTimeCounter = coyoteTime;
            wasInAir = false; // Reset after landing
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            wasInAir = true; // Player is in the air
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
    private IEnumerator TriggerRumble(float lowFrequency, float highFrequency, float duration)
    {
        Gamepad gamepad = Gamepad.current;
        if (gamepad != null)
        {
            //use gamepad motor speeds and adjust where, needed 
            //this routine should start and stop the gamepad motors 
            gamepad.SetMotorSpeeds(lowFrequency, highFrequency);
            yield return new WaitForSeconds(duration);
            gamepad.SetMotorSpeeds(0, 0); 
        }
    }

    void MoveTile()
    {
                
    }

}
 
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialAudio : MonoBehaviour
{
    public AudioSource Source;
    public AudioClip WelcomeClip;
    public AudioClip JumpClip;
    public AudioClip MoveTileUp;
    public AudioClip MoveTileDown;
    public AudioClip ClosingClip;
    public PlayerController playerController;
    public GameObject Player;
    public PlayerInput playerInput;
    public bool isMouse;
    public bool isController;

    private bool JumpClipPlayed = false;
    private bool RaiseClipPlayed = false;
    private bool LowerClipPlayed = false;

    void Start()
    {
        playerInput = Player.GetComponent<PlayerInput>();
        DisableController();
        Source.clip = WelcomeClip;
        Source.Play();

        isMouse = playerInput.currentControlScheme == "Keyboard&Mouse";
        isController = playerInput.currentControlScheme == "Gamepad";

        // Subscribe to events once
        playerInput.actions["Move"].performed += OnMovePerformed;
        playerInput.actions["Jump"].performed += OnJumpPerformed;
        playerInput.actions["TileUp"].performed += OnTileUpPerformed;
        playerInput.actions["TileDown"].performed += OnTileDownPerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        if (!JumpClipPlayed && !Source.isPlaying)
        {
            EnableController();
            Source.clip = JumpClip;
            Source.Play();
            JumpClipPlayed = true;
        }
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        if (!RaiseClipPlayed && !Source.isPlaying && Source.clip == JumpClip)
        {
            Source.clip = MoveTileUp;
            Source.Play();
            RaiseClipPlayed = true;
        }
    }

    private void OnTileUpPerformed(InputAction.CallbackContext ctx)
    {
        if (!LowerClipPlayed && !Source.isPlaying && Source.clip == MoveTileUp)
        {
            Source.clip = MoveTileDown;
            Source.Play();
            LowerClipPlayed = true;
        }
    }

    private void OnTileDownPerformed(InputAction.CallbackContext ctx)
    {
        if (!Source.isPlaying && Source.clip == MoveTileDown)
        {
            Source.clip = ClosingClip;
            Source.Play();
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        playerInput.actions["Move"].performed -= OnMovePerformed;
        playerInput.actions["Jump"].performed -= OnJumpPerformed;
        playerInput.actions["TileUp"].performed -= OnTileUpPerformed;
        playerInput.actions["TileDown"].performed -= OnTileDownPerformed;
    }

    public void DisableController()
    {
        playerController.enabled = false;
    }

    public void EnableController()
    {
        playerController.enabled = true;
    }
}

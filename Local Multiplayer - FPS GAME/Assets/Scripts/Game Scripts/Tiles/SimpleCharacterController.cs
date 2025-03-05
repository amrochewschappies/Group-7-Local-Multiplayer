using UnityEngine;

public class SimpleCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;      // Movement speed of the character
    public float rotationSpeed = 100f; // Rotation speed of the character

    private CharacterController controller; // Reference to the CharacterController component

    void Start()
    {
        controller = GetComponent<CharacterController>(); // Get the CharacterController component
    }

    void Update()
    {
        // Get input for movement (WASD or Arrow keys)
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow (but we use only WASD for movement)
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow (but we use only WASD for movement)

        // Create a movement vector based on input
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Move the character (no Y movement since we're not handling jumping)
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Get input for rotation (Arrow keys only)
        float rotateX = Input.GetAxis("Horizontal"); // Left/Right Arrow
        float rotateY = Input.GetAxis("Vertical");   // Up/Down Arrow

        // Rotate the character using only the Arrow Keys
        if (rotateX != 0f)
        {
            transform.Rotate(Vector3.up, rotateX * rotationSpeed * Time.deltaTime); // Y-axis rotation
        }
        if (rotateY != 0f)
        {
            transform.Rotate(Vector3.right, -rotateY * rotationSpeed * Time.deltaTime); // X-axis rotation (inverted)
        }
    }
}

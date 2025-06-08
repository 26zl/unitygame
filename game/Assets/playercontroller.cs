using UnityEngine;
using UnityEngine.InputSystem; // You need this for the new Input System

// The file name must be "PlayerController.cs" to match this class name
public class PlayerController : MonoBehaviour
{
    // --- Public variables (can be adjusted in the Inspector) ---

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Camera Look")]
    public Camera playerCamera; // Drag your Main Camera here in the Inspector
    public float lookSpeed = 2.0f;
    public float lookXLimit = 80.0f; // Limit for looking up and down

    // --- Private variables (for internal script use) ---

    private Rigidbody rb;
    private Vector2 moveInput;  // Stores input from WASD/Left Stick
    private Vector2 lookInput;  // Stores input from Mouse/Right Stick
    private float xRotation = 0f; // Stores the current up/down rotation of the camera
    private bool isGrounded;    // Check if the player is on the ground

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        // Get the Rigidbody component attached to this GameObject for physics
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Lock the cursor to the center of the screen and hide it for a clean FPS feel
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // --- Input System Message Handlers ---
    // These methods are called by the "Player Input" component's "Send Messages" behavior.
    // The method name (e.g., "OnMove") must match the Action name in your Input Asset.

    public void OnMove(InputValue value)
    {
        // Read the Vector2 value from the input action
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        // Read the Vector2 value from the input action
        lookInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        // Only jump if the button is pressed AND the player is on the ground
        if (value.isPressed && isGrounded)
        {
            // Apply an instant upward force using an Impulse
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // --- Update and FixedUpdate ---

    // FixedUpdate is called at a fixed interval and is used for physics calculations.
    void FixedUpdate()
    {
        // Create a 3D movement vector based on the 2D input
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Convert the local movement direction (relative to the player) to a world-space direction
        Vector3 worldMove = transform.TransformDirection(move);

        // Apply movement to the Rigidbody's velocity.
        // We preserve the current Y velocity to not interfere with jumping or gravity.
        rb.linearVelocity = new Vector3(worldMove.x * moveSpeed, rb.linearVelocity.y, worldMove.z * moveSpeed);
    }

    // Update is called once per frame. Best for non-physics logic like camera look.
    void Update()
    {
        // Calculate camera rotation for looking up and down
        xRotation -= lookInput.y * lookSpeed;
        xRotation = Mathf.Clamp(xRotation, -lookXLimit, lookXLimit); // Prevent flipping the camera

        // Apply the up/down rotation directly to the camera's local transform
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Apply the left/right rotation to the entire player body's transform
        transform.Rotate(Vector3.up * lookInput.x * lookSpeed);
    }

    // --- Ground Check ---
    // This is a simple way to check if the player is touching the ground.

    void OnCollisionStay(Collision collision)
    {
        // Check if the surface we are touching is mostly flat (i.e., it's a floor, not a wall)
        if (Vector3.Dot(collision.contacts[0].normal, Vector3.up) > 0.5)
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // When we stop touching anything, we are no longer grounded
        isGrounded = false;
    }
}
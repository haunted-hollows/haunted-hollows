using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Import movement actions from PlayerControls script
    public PlayerControls controls;

    // Script flags
    public bool DEBUG_MODE = false;

    // Propreties
    public float jumpNumber = 2; // Number of jumps
    public float speed = 2.5f;
    public float runningMultiplier = 1.5f;
    public float raycastDownDistance = 1f; // How far to raycast down
    public float jumpForce = 8f;
    public float lerpRate = 0.15f;
    public LayerMask groundLayer;
    [HideInInspector] // Hide in unity propreties
    public Camera mainCam;
    [HideInInspector] // Hide in unity propreties
    public bool steer;

    private bool isRunning;
    private bool isGrounded;
    private float availableJumps;
    private float speedMultiplier;

    // References
    private Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize environment
        rigidBody = gameObject.GetComponent<Rigidbody>();
        isGrounded = false;
        isRunning = false;
        speedMultiplier = 1;
        availableJumps = jumpNumber;
    }

    // Update is called once per frame
    void Update()
    {
        // Fetch horizontal and vertical input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Running logic
        isRunning = Input.GetKey(KeyCode.LeftShift);
        speedMultiplier = (isRunning) ? (speed * runningMultiplier) : speed;

        // Groundcheck by sending a raycast downwards
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out _, raycastDownDistance, groundLayer);

        // Handle double jumping 
        if (isGrounded)
        {
            availableJumps = jumpNumber;
        }

        // Create a direction vector based on the input
        Vector3 direction = new Vector3(horizontal, 0, vertical);
        if (direction.magnitude > 1)
        {
            direction /= direction.magnitude;
        }

        // Movement
        if (direction.magnitude >= 0.1f)
        {
            Vector3 targetPosition = rigidBody.position + direction * (speed * speedMultiplier) * Time.deltaTime;
            rigidBody.MovePosition(Vector3.Lerp(rigidBody.position, targetPosition, lerpRate));
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && availableJumps > 0)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
            availableJumps--;
        }

        // Print debug info if we are in debug mode
        if (DEBUG_MODE)
            DebugPrint();
    }

    // Debug print function
    void DebugPrint()
    {
        Debug.Log((object)("SPEED:::", speed));
        Debug.Log((object)("RUNNING_MULTIPLIER:::", runningMultiplier));
        Debug.Log((object)("SPEED_MULTIPLIER:::", speedMultiplier));
        Debug.Log((object)("JUMP_FORCE:::", jumpForce));
        Debug.Log((object)("AVAILABLE_JUMPS:::", availableJumps));
        Debug.Log((object)("IS_RUNNING:::", isRunning));
        Debug.Log((object)("IS_GROUNDED:::", isGrounded));
    }
}
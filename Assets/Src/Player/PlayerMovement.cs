using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Import movement actions from PlayerControls script
    public PlayerControls controls;

    // Script flags
    public bool DEBUG_MODE = false;

    // Properties
    public float jumpNumber = 2; // Number of jumps
    public float speed = 8f;
    public float runningMultiplier = 1.5f;
    public float raycastDownDistance = 0.7f; // How far to raycast down
    public float jumpForce = 8f;
    public float lerpRate = 0.15f;
    public LayerMask groundLayer;
    [HideInInspector] // Hide in unity properties
    public Camera mainCam;
    [HideInInspector] // Hide in unity properties
    public bool steer;

    private bool isRunning;
    private bool isGrounded;
    private float availableJumps;
    private float speedMultiplier;
    private bool hasDoubleJumped = false;

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

    void Update()
    {
        // Fetch horizontal and vertical input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Running logic
        isRunning = Input.GetKey(KeyCode.LeftShift);
        speedMultiplier = (isRunning) ? (speed * runningMultiplier) : speed;

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

        // Jump Logic
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
                hasDoubleJumped = false;
            }
            else if (!hasDoubleJumped)
            {
                Jump();
                hasDoubleJumped = true;
            }

            if (rigidBody.velocity.y > 0f)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);
            }
        }

        if (isGrounded)
        {
            availableJumps = jumpNumber;
        }

        // Print debug info if we are in debug mode
        if (DEBUG_MODE)
            DebugPrint();
    }

    void Jump()
    {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        availableJumps--;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    // Debug print function
    void DebugPrint()
    {
        Debug.Log((object)("AVAILABLE_JUMPS:::", availableJumps));
    }
}

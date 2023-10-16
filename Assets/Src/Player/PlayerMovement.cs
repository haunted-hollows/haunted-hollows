using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Script flags
    public bool DEBUG_MODE = false;
    
    // Propreties
    public float airJumps = 1; // Number of air jumps
    public float speed = 2.5f;
    public float runningMultiplier = 1.5f;
    public float raycastDownDistance = 1f; // How far to raycast down
    public float jumpForce = 8f;
    public LayerMask groundLayer;

    private bool frameJump;
    private bool isRunning;
    private bool isGrounded;
    private float availableJumps;
    private float speedMultiplier;
    private float ySpeed;
    private float xSpeed;
    private Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize environment
        rigidBody = gameObject.GetComponent<Rigidbody>();
        isGrounded = false;
        isRunning = false;
        speedMultiplier = 1;
        xSpeed = 0;
        ySpeed = 0;
        availableJumps = airJumps;
    }

    // Update is called once per frame
    void Update()
    {
        // Running logic
        isRunning = Input.GetKey(KeyCode.LeftShift); 
        speedMultiplier = (isRunning)? (speed * runningMultiplier): speed;

        // Update x & y speeds each frame, also add value to y gravity to make it heavier
        ySpeed += (Physics.gravity.y - 3) * (Time.deltaTime);
        xSpeed += Physics.gravity.x * Time.deltaTime;

        // Groundcheck by sending a raycast downwards
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out _, raycastDownDistance, groundLayer);

        // Handle double jumping 
        if (isGrounded) {
            availableJumps = airJumps;
        } 

        // Key handling
        if (Input.GetKey(KeyCode.D)) {
            transform.position += Vector3.right * (speed * speedMultiplier) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.position += Vector3.left * (speed * speedMultiplier) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W)) {
            transform.position += Vector3.forward * (speed * speedMultiplier) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.position += Vector3.back * (speed * speedMultiplier) * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space) && availableJumps > 0) {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
        Debug.Log((object)("X_SPEED:::", xSpeed)); 
        Debug.Log((object)("Y_SPEED:::", ySpeed)); 
    }
}

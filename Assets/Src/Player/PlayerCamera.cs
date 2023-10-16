using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Propreties
    public GameObject player;
		public float sensitivity;

    private Vector3 offset;

    // Use this for initialization
    void Start() 
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate() 
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = player.transform.position + offset;
    }

		// Fixed update
		void FixedUpdate ()
		{
				float rotateHorizontal = Input.GetAxis ("Mouse X");
				float rotateVertical = Input.GetAxis ("Mouse Y");
				transform.RotateAround (player.transform.position, -Vector3.up, rotateHorizontal * sensitivity); 
				transform.RotateAround (Vector3.zero, transform.right, rotateVertical * sensitivity); 
		}
}

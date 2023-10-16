using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Propreties
    public float CameraHeight = 2f;
    public float CameraMaxDistance = 25f;
    public float CameraMaxTilt = 90f;
    [Range(0, 4)]
    public float CameraSpeed = 2f;
    public Transform tilt;
    
    private float currentPan;
    private float currentTilt;
    private float currentDistance; // How far zoomed out
    private Camera mainCam;
    private CameraState cameraState;
    private PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize environment
        player = FindObjectOfType<PlayerMovement>();
        //player.mainCam = this;
        mainCam = Camera.main;
        currentTilt = 10f;
        currentDistance = 5f;
        cameraState = CameraState.CameraNone;
        
        // Initialize camera position & rotation based on player
        transform.position = player.transform.position + Vector3.up * CameraHeight;
        transform.rotation = player.transform.rotation;

        // Initialize camera tilt
        tilt.eulerAngles = new Vector3(currentTilt, transform.eulerAngles.y, transform.eulerAngles.z);
        mainCam.transform.position += tilt.forward * -currentDistance;
    }

    // Update is called once per frame
    void Update()
    {
        // left:Mouse0, right:Mouse1, middle:Mouse2
        // If left mouse
        if (Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Mouse1) && !Input.GetKey(KeyCode.Mouse2))
            cameraState = CameraState.CameraRotate;
        // If right mouse
        else if (!Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1) && !Input.GetKey(KeyCode.Mouse2))
            cameraState = CameraState.CameraSteer;
        else
            cameraState = CameraState.CameraNone;

        CameraInputs();
    }

    // This is called in the end of Update()
    void LateUpdate()
    {
        CameraTransforms();
    }

    // Apply camera steering/rotating
    void CameraInputs()
    {
        if (cameraState != CameraState.CameraNone)   
        {
            if (cameraState == CameraState.CameraRotate)
            {
                currentPan += Input.GetAxis("Mouse X") * CameraSpeed;

                if (player.steer)
                    player.steer = false;
            }
            else if (cameraState == CameraState.CameraSteer || cameraState == CameraState.CameraRun)
            {
                if (!player.steer)
                    player.steer = true;
            }

            currentTilt -= Input.GetAxis("Mouse Y") * CameraSpeed;
            currentTilt = Math.Clamp(currentTilt, -CameraMaxTilt, CameraMaxTilt);
        }

        currentDistance -= Input.GetAxis("Mouse ScrollWheel") * 2;
        currentDistance = Math.Clamp(currentDistance, 0, CameraMaxDistance);
    }

    // Apply transformations to camera based on player current position
    void CameraTransforms()
    {
        switch (cameraState)
        {
            case CameraState.CameraSteer:
            case CameraState.CameraRun:
            case CameraState.CameraNone:
                currentPan = player.transform.eulerAngles.y;
                break;
        }
        
        if (cameraState == CameraState.CameraNone)
            currentTilt = 10;

        // Rotation
        currentPan = player.transform.eulerAngles.y;

        // Position
        transform.position = player.transform.position + Vector3.up * CameraHeight;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentPan, transform.eulerAngles.z);
        tilt.eulerAngles = new Vector3(currentTilt, transform.eulerAngles.y, transform.eulerAngles.z);
        mainCam.transform.position = transform.position + tilt.forward * -currentDistance;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // What To Look At
    public Transform whatToLookAt;

    // Camera Settings
    // - For Developers
    private const float YMin = -30.0f;
    private const float YMax = 80.0f;

    // - For Players
    public float distance = 3.0f;
    public float sensivity = 50.0f;

    // Mouse Position Tracking
    private float currentX = 0.0f;
    private float currentY = 0.0f;

    // cursor management
    private bool lockedCursor;
    private bool cursorFlip;


    void Start()
    {
        // Hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        lockedCursor = true;
        cursorFlip = false;
    }

    void Update()
    {
        // control cursor locking
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            cursorFlip = true;
        }
        if (cursorFlip)
        {
            if (lockedCursor)
            {
                Cursor.lockState = CursorLockMode.Confined;
                lockedCursor = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                lockedCursor = true;
            }
            cursorFlip = false;
        }
    }

    void LateUpdate()
    {
        if (lockedCursor)
        {
            // Get Mouse Position
            currentX += Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
            currentY += Input.GetAxis("Mouse Y") * -sensivity * Time.deltaTime;
            currentY = Mathf.Clamp(currentY, YMin, YMax); // Limit vertical Position
        }
        // Set Camera Position
        Vector3 Location = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = whatToLookAt.position + (rotation * Location);

        // Face Camera to focus point
        transform.LookAt(whatToLookAt.position);
    }
}

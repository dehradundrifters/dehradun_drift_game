using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class XRTeleportControllerWithLookAt : MonoBehaviour
{
    public InputActionReference XButtonAction;  // Reference for the X Button
    public InputActionReference YButtonAction;  // Reference for the Y Button
    public XROrigin xrOrigin;                  // Reference to the XR Origin
    public List<Transform> teleportPoints;     // List of transforms to teleport to
    public Transform centerObject;             // The center GameObject to look at

    private Vector3 previousPosition;          // Store the XR Origin's previous position
    private int currentPointIndex = 0;         // Index of the current teleport point
    private bool isTeleported = false;         // Track whether the player is currently teleported

    void OnEnable()
    {
        // Enable button input actions
        XButtonAction.action.Enable();
        YButtonAction.action.Enable();

        // Subscribe to button events
        XButtonAction.action.performed += OnAButtonPressed;
        YButtonAction.action.performed += OnBButtonPressed;
    }

    void OnDisable()
    {
        // Unsubscribe from button events
        XButtonAction.action.performed -= OnAButtonPressed;
        YButtonAction.action.performed -= OnBButtonPressed;

        // Disable button input actions
        XButtonAction.action.Disable();
        YButtonAction.action.Disable();
    }

    private void OnAButtonPressed(InputAction.CallbackContext context)
    {
        if (!isTeleported)
        {
            // Save the previous position
            previousPosition = xrOrigin.transform.position;

            // Teleport to the first point in the list
            TeleportToPoint(0);

            isTeleported = true;
        }
        else
        {
            // Return to the previous position
            xrOrigin.transform.position = previousPosition;

            isTeleported = false;
        }
    }

    private void OnBButtonPressed(InputAction.CallbackContext context)
    {
        if (isTeleported)
        {
            // Move to the next point in the list
            currentPointIndex = (currentPointIndex + 1) % teleportPoints.Count;

            // Teleport to the current point
            TeleportToPoint(currentPointIndex);
        }
    }

    private void TeleportToPoint(int pointIndex)
    {
        // Ensure the index is valid
        if (pointIndex >= 0 && pointIndex < teleportPoints.Count)
        {
            // Teleport to the target position
            xrOrigin.transform.position = teleportPoints[pointIndex].position;

            // Make the XR Origin look at the center object
            if (centerObject != null)
            {
                Vector3 lookDirection = centerObject.position - xrOrigin.Camera.transform.position;
                lookDirection.y = 0f; // Keep the look direction horizontal
                xrOrigin.transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
        else
        {
            Debug.LogWarning("Invalid teleport point index: " + pointIndex);
        }
    }
}

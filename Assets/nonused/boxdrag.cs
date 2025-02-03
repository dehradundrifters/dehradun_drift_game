using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GrabAndMoveCube : MonoBehaviour
{
    private bool isHeld = false; // Whether the cube is currently being held
    private Transform handTransform; // The transform of the hand holding the cube
    private Rigidbody cubeRigidbody; // The cube's Rigidbody

    void Start()
    {
        // Get the cube's Rigidbody component
        cubeRigidbody = GetComponent<Rigidbody>();

        if (cubeRigidbody == null)
        {
            Debug.LogError("No Rigidbody found on the cube. Please attach a Rigidbody component.");
        }
    }

    void Update()
    {
        // If the cube is being held, follow the hand's position and rotation
        if (isHeld && handTransform != null)
        {
            // Set the position and rotation of the cube to the hand's transform
            transform.position = handTransform.position;
            transform.rotation = handTransform.rotation;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the hand (you can tag the hand object as "Hand" in Unity)
        if (other.CompareTag("Hand"))
        {
            // Store the hand's transform
            handTransform = other.transform;

            // Check if the trigger button is pressed while the hand is in contact with the cube
            if (InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed))
            {
                if (triggerPressed)
                {
                    // Begin holding the cube
                    StartHoldingCube();
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Continue checking if the trigger is pressed while the hand is in contact with the cube
        if (other.CompareTag("Hand"))
        {
            if (InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed))
            {
                if (triggerPressed && !isHeld)
                {
                    // Begin holding the cube
                    StartHoldingCube();
                }
                else if (!triggerPressed && isHeld)
                {
                    // Release the cube when the trigger is released
                    StopHoldingCube();
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Stop holding the cube if the hand moves away from the cube
        if (other.CompareTag("Hand") && isHeld)
        {
            StopHoldingCube();
        }
    }

    void StartHoldingCube()
    {
        isHeld = true;
        // Disable gravity so the cube doesn't fall while being held
        cubeRigidbody.useGravity = false;
        cubeRigidbody.isKinematic = true; // Make the cube kinematic so it follows the hand's transform
    }

    void StopHoldingCube()
    {
        isHeld = false;
        // Enable gravity again so the cube falls naturally when released
        cubeRigidbody.useGravity = true;
        cubeRigidbody.isKinematic = false; // Make the cube non-kinematic so physics can be applied again
        handTransform = null; // Clear the reference to the hand's transform
    }
}

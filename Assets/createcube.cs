using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InstantiateCubeAtRightHandModel : MonoBehaviour
{
    public GameObject cubePrefab;   // Assign a cube prefab in the Inspector
    public Transform rightHandModel;  // Assign the right-hand model's transform (e.g., RightHandAnchor)

    private bool isButtonPressed = false; // Flag to track button press status

    void Update()
    {
        // Check if the right-hand controller primary button (A button) is pressed
        if (InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out bool buttonPressed))
        {
            if (buttonPressed && !isButtonPressed)
            {
                // Button just pressed, instantiate the cube
                InstantiateCubeAtHand();
                isButtonPressed = true;
            }
            else if (!buttonPressed)
            {
                // Button released, reset the flag
                isButtonPressed = false;
            }
        }
    }

    void InstantiateCubeAtHand()
    {
        // Check if right hand model is assigned
        if (rightHandModel != null)
        {
            // Instantiate the cube at the right hand model's position and rotation
            Instantiate(cubePrefab, rightHandModel.position, rightHandModel.rotation);
        }
        else
        {
            Debug.LogWarning("Right hand model transform not assigned.");
        }
    }
}

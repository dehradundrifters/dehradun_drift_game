using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.XR.Content.Interaction;

public class RemoteSteeringControl : MonoBehaviour
{
    [Header("References")]
    public XRKnob steeringWheel; // The XR Knob (Steering Wheel)

    [Header("Input Actions")]
    public InputActionReference leftGripAction;
    public InputActionReference rightGripAction;

    [Header("XR Controllers")]
    public Transform leftHand;
    public Transform rightHand;

    [Header("Settings")]
    public float twistSensitivity = 1.0f; // Adjusts how much hand movement affects rotation

    private Transform currentHand;
    private bool isGrabbing = false;
    private float initialAngle;
    private Vector3 initialLocalHandPosition;

    void Start()
    {
        if (steeringWheel == null)
        {
            steeringWheel = GetComponent<XRKnob>();
        }

        // Start the knob at the center (0.5 of min/max range)
        steeringWheel.value = Mathf.Lerp(steeringWheel.minAngle, steeringWheel.maxAngle, 0.5f);
    }

    void Update()
    {
        bool leftGripPressed = leftGripAction.action.ReadValue<float>() > 0.5f;
        bool rightGripPressed = rightGripAction.action.ReadValue<float>() > 0.5f;

        if (!isGrabbing)
        {
            if (leftGripPressed)
            {
                StartGrabbing(leftHand);
            }
            else if (rightGripPressed)
            {
                StartGrabbing(rightHand);
            }
        }
        else
        {
            if (!leftGripPressed && !rightGripPressed)
            {
                StopGrabbing();
            }
            else
            {
                RotateWheelWithHand();
            }
        }
    }

    private void StartGrabbing(Transform hand)
    {
        isGrabbing = true;
        currentHand = hand;

        // Save the hand's LOCAL position relative to the steering wheel
        initialLocalHandPosition = steeringWheel.transform.InverseTransformPoint(hand.position);
        initialAngle = steeringWheel.value;  // Store the current knob angle at grab time
    }

    private void RotateWheelWithHand()
    {
        if (currentHand != null)
        {
            // Get the current local position of the hand relative to the steering wheel
            Vector3 currentLocalHandPosition = steeringWheel.transform.InverseTransformPoint(currentHand.position);

            // Calculate the movement difference in the X-axis (left-right movement)
            float handDelta = currentLocalHandPosition.x - initialLocalHandPosition.x;

            // Apply rotation based on hand movement and sensitivity
            float rotationAmount = handDelta * twistSensitivity * 100f;

            // Clamp the rotation to stay within the valid knob range
            steeringWheel.value = Mathf.Clamp(initialAngle + rotationAmount, steeringWheel.minAngle, steeringWheel.maxAngle);
        }
    }

    private void StopGrabbing()
    {
        isGrabbing = false;
        currentHand = null;
    }
}

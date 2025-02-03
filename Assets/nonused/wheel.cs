using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Content.Interaction;

public class WheelScript : MonoBehaviour
{
    public GameObject obj;             // The object that will rotate and move
    public XRKnob knob;                // The knob used for rotation control
    public InputActionProperty accelerator; // Input action for moving forward
    public InputActionProperty brake;        // Input action for moving backward

    public float speed = 5f;           // Speed multiplier for movement

    private void Update()
    {
        // Get the rotation from the knob and apply it to the object
        float yRotation = knob.value * 360f;
        obj.transform.rotation = Quaternion.Euler(0, yRotation, 0);

        // Get the forward direction based on the current y-rotation
        Vector3 forwardDirection = obj.transform.forward;

        // Check if the accelerator or brake input actions are pressed
        if (accelerator.action.ReadValue<float>() > 0.1f)
        {
            // Move forward
            obj.transform.position += forwardDirection * speed * Time.deltaTime;
        }
        else if (brake.action.ReadValue<float>() > 0.1f)
        {
            // Move backward
            obj.transform.position -= forwardDirection * speed * Time.deltaTime;
        }
    }
}

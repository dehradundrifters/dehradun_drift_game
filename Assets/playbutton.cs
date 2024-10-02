using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.InputSystem; // Ensure the Input System package is installed

public class SplineAnimateController : MonoBehaviour
{
    public SplineAnimate splineAnimate; // Reference to the SplineAnimate component
    public InputActionProperty xButtonAction; // Input Action for the X button on the left controller
    public GameObject xrorigincamera;
    public GameObject playercamera;
    private void Start()
    {
        xrorigincamera.SetActive(true) ;
        playercamera.SetActive(false) ;
    }
    void Update()
    {
        // Check if the X button on the left controller is pressed
        if (xButtonAction.action.WasPressedThisFrame())
        {
            if (!splineAnimate.isPlaying)
            {
                xrorigincamera.SetActive(false);
                playercamera.SetActive(true);
                splineAnimate.Play(); // Play the Spline Animate if it is not already playing
                Debug.Log("X button pressed, playing Spline Animate.");
            }
            else
            {
                splineAnimate.Pause(); // Optionally pause if it's already playing
                Debug.Log("X button pressed, pausing Spline Animate.");
            }
        }
    }
}

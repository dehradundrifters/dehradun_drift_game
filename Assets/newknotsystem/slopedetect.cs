using UnityEngine;
using UnityEngine.Splines;

public class SlopeDetector : MonoBehaviour
{
    private SplineAnimate splineAnimate;
    public SplineContainer splineContainer; // Publicly assign the SplineContainer in the Inspector

    private Vector3 previousPosition;
    public float assignedSpeed; // Variable to hold the dynamically changing speed
    public float speedIncrementFactor = 0.1f; // Factor to increase speed on downslope
    public float speedDecrementFactor = 0.1f; // Factor to decrease speed on upslope
    public float minSpeed = 3f; // Minimum allowed speed

    void Start()
    {
        // Automatically find the SplineAnimate component on the same GameObject
        splineAnimate = GetComponent<SplineAnimate>();

        // Check if SplineAnimate and SplineContainer are available
        if (splineAnimate == null)
        {
            Debug.LogError("SplineAnimate component not found on this GameObject!");
        }
        if (splineContainer == null)
        {
            Debug.LogError("SplineContainer is not assigned! Please assign it in the Inspector.");
        }

        // Initialize the previous position and assignedSpeed with maxSpeed from SplineAnimate
        previousPosition = splineContainer.EvaluatePosition(splineAnimate.normalizedTime);
        assignedSpeed = splineAnimate.maxSpeed; // Assign the maxSpeed to the assignedSpeed initially
    }

    void Update()
    {
        // Proceed only if both components are available
        if (splineAnimate != null && splineContainer != null)
        {
            // Get the current normalized time of the SplineAnimate (0 to 1)
            float currentProgress = splineAnimate.normalizedTime;

            // Get the current position on the spline
            Vector3 currentPosition = splineContainer.EvaluatePosition(currentProgress);

            // Calculate the movement direction (difference between current and previous positions)
            Vector3 movementDirection = currentPosition - previousPosition;

            // Calculate the angle of movement relative to the horizontal (XZ plane)
            float angle = Mathf.Atan2(movementDirection.y, movementDirection.magnitude) * Mathf.Rad2Deg;

            // Adjust the speed based on the slope
            if (movementDirection.y > 0) // Upslope detected
            {
                Debug.Log($"Upslope detected. Angle: {angle}°");
                assignedSpeed -= Mathf.Abs(angle) * speedDecrementFactor; // Decrease speed
                assignedSpeed = Mathf.Clamp(assignedSpeed, minSpeed, splineAnimate.maxSpeed); // Prevent going below minSpeed
            }
            else if (movementDirection.y < 0) // Downslope detected
            {
                Debug.Log($"Downslope detected. Angle: {angle}°");
                assignedSpeed += Mathf.Abs(angle) * speedIncrementFactor; // Increase speed
                assignedSpeed = Mathf.Clamp(assignedSpeed, minSpeed, splineAnimate.maxSpeed * 2); // Ensure it doesn't go below minSpeed
            }
            else // Flat surface detected
            {
                Debug.Log($"Flat surface detected. Angle: {angle}°");
                // You can add additional logic for flat surfaces if needed
            }

            // Log the current speed
            Debug.Log($"Current assignedSpeed: {assignedSpeed}");

            // Update the speed of SplineAnimate (optional)
            splineAnimate.MaxSpeed = assignedSpeed;

            // Update the previous position for the next frame
            previousPosition = currentPosition;
        }
    }
}

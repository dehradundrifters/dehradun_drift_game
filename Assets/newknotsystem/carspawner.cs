using UnityEngine;
using UnityEngine.Splines;

public class SetActiveOnSpline : MonoBehaviour
{
    public SplineContainer splineContainer; // Assign this in the Inspector
    public GameObject targetGameObject; // The GameObject you want to activate and move
    public float yOffset = 1.0f; // Offset in the Y-axis
    public float zOffset = 1.0f; // Dynamic offset along the direction of the spline
    public GameObject camerarig;

    public void spawnthecar()
    {
        if (splineContainer == null || targetGameObject == null)
        {
            Debug.LogError("SplineContainer or Target GameObject is not assigned.");
            return;
        }

        // Get the first spline (spline 0) from the container
        if (splineContainer.Splines.Count > 0)
        {
            Spline spline = splineContainer.Splines[0]; // Access the first spline

            // Ensure the spline has at least 2 knots (knot 0 and knot 1)
            if (spline.Count > 1)
            {
                // Get the world position of knot 0
                Vector3 knot0Position = spline[0].Position;
                Vector3 worldKnot0Position = splineContainer.transform.TransformPoint(knot0Position);

                // Get the world position of knot 1
                Vector3 knot1Position = spline[1].Position;
                Vector3 worldKnot1Position = splineContainer.transform.TransformPoint(knot1Position);

                // Compute the direction vector from knot 0 to knot 1
                Vector3 forwardDirection = (worldKnot1Position - worldKnot0Position).normalized;

                // Apply the offset in the direction of the second knot
                worldKnot0Position += forwardDirection * zOffset;

                // Apply Y offset
                worldKnot0Position.y += yOffset;

                // Move the target GameObject to knot 0's world position with offsets
                targetGameObject.transform.position = worldKnot0Position;

                // Make the GameObject look at knot 1's world position
                targetGameObject.transform.LookAt(worldKnot1Position);

                // Disable the camera rig and activate the car
                camerarig.SetActive(false);
                targetGameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("The selected spline does not contain enough knots.");
            }
        }
        else
        {
            Debug.LogWarning("The SplineContainer does not contain any splines.");
        }
    }
}

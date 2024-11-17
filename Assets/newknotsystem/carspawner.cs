using UnityEngine;
using UnityEngine.Splines;

public class SetActiveOnSpline : MonoBehaviour
{
    public SplineContainer splineContainer; // Assign this in the Inspector
    public GameObject targetGameObject; // The GameObject you want to activate and move
    public float yOffset = 1.0f; // Offset in the Y-axis
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
                // Get the position of knot 0
                Vector3 knot0Position = spline[0].Position; // Local position of knot 0
                Vector3 worldKnot0Position = splineContainer.transform.TransformPoint(knot0Position); // Convert to world position

                // Add the Y-axis offset to knot 0 position
                worldKnot0Position.y += yOffset;

                // Get the position of knot 1
                Vector3 knot1Position = spline[1].Position; // Local position of knot 1
                Vector3 worldKnot1Position = splineContainer.transform.TransformPoint(knot1Position); // Convert to world position

                // Move the target GameObject to knot 0's world position
                targetGameObject.transform.position = worldKnot0Position;

                // Make the GameObject look at knot 1's world position
                targetGameObject.transform.LookAt(worldKnot1Position);
                camerarig.SetActive(false);
                // Set the GameObject active
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
    


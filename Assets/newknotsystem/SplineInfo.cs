using UnityEngine;
using UnityEngine.Splines;

public class SplineInfo : MonoBehaviour
{
    public SplineContainer splineContainer; // Reference to the SplineContainer
    void Update()
    {
        // Check if B is pressed to show spline and knot information
        if (Input.GetKeyDown(KeyCode.J))
        {
            onlyreinitialize();
        }

        // Check if J is pressed to reinitialize and delete the last knot GameObject

    }
    public void onlyreinitialize()
    {
        int knotIndex = 0;

        // Get all children with KnotMover scripts
        KnotMover[] knotMovers = GetComponentsInChildren<KnotMover>();

        // Iterate through each spline in the SplineContainer
        for (int splineIndex = 0; splineIndex < splineContainer.Splines.Count; splineIndex++)
        {
            Spline spline = splineContainer.Splines[splineIndex];
            int knotsInCurrentSpline = spline.Count;

            // Reinitialize knot indices for the current spline
            for (int i = 0; i < knotsInCurrentSpline; i++)
            {
                // Ensure we don't go out of bounds
                if (knotIndex < knotMovers.Length)
                {
                    KnotMover knotMover = knotMovers[knotIndex];
                    knotMover.Initialize(i, splineContainer, splineIndex); // Assign the knotIndex within the current spline
                    knotIndex++; // Move to the next knot in the hierarchy
                }
            }
        }

        Debug.Log("Knots and splines reinitialized.");
    }
    public void SetBeforeJoinBoolFalse()
    {
        // Get all children with KnotMover scripts
        KnotMover[] knotMovers = GetComponentsInChildren<KnotMover>();

        // Iterate through each KnotMover and set beforejoinbool to false
        foreach (KnotMover knotMover in knotMovers)
        {
            //knotMover.beforejoinbool = false;
        }

        Debug.Log("Set beforejoinbool to false for all KnotMover components.");
    }
    // Function to reinitialize KnotMover scripts on all child GameObjects
    public void ReinitializeKnots(GameObject caller)
    {
        int knotIndex = 0;
        KnotMover[] knotMovers = GetComponentsInChildren<KnotMover>();

        for (int splineIndex = 0; splineIndex < splineContainer.Splines.Count; splineIndex++)
        {
            Spline spline = splineContainer.Splines[splineIndex];
            int knotsInCurrentSpline = spline.Count;

            for (int i = 0; i < knotsInCurrentSpline; i++)
            {
                // Skip the caller GameObject
                if (knotIndex < knotMovers.Length && knotMovers[knotIndex].gameObject != caller)
                {
                    KnotMover knotMover = knotMovers[knotIndex];
                    knotMover.Initialize(i, splineContainer, splineIndex);
                }
                else if (knotIndex < knotMovers.Length && knotMovers[knotIndex].gameObject == caller)
                {
                    // Increment the index without assigning to skip the caller
                    i--; // Adjust the spline knot count for the skipped GameObject
                }

                knotIndex++;
            }
        }

        Debug.Log("Knots and splines reinitialized, ignoring the caller.");
    }
}

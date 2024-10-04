using UnityEngine;
using UnityEngine.Splines;

public class SplineLinker : MonoBehaviour
{
    private KnotMover knotMover;

    private void Start()
    {
        // Get the KnotMover component attached to this sphere
        knotMover = GetComponent<KnotMover>();
        if (knotMover == null)
        {
            Debug.LogError("KnotMover component not found on this object.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object has the "knot" tag
        if (other.CompareTag("knot"))
        {
            Debug.Log("Checking if knots can be linked");
            // Get the KnotMover component from the other sphere
            KnotMover otherKnotMover = other.GetComponent<KnotMover>();

            if (otherKnotMover != null)
            {
                // Access the SplineContainer and spline indices from both objects
                SplineContainer currentContainer = knotMover.splineContainer;

                SplineKnotIndex knotA = new SplineKnotIndex(knotMover.splineIndex, knotMover.knotIndex);
                SplineKnotIndex knotB = new SplineKnotIndex(otherKnotMover.splineIndex, otherKnotMover.knotIndex);

                // Only link if the knots are on different splines
                if (knotA.Spline != knotB.Spline)
                {
                    // Link the two knots
                    currentContainer.LinkKnots(knotA, knotB);

                    Debug.Log($"Knots linked: Knot {knotA.Knot} of Spline {knotA.Spline} with Knot {knotB.Knot} of Spline {knotB.Spline}.");
                }
                else
                {
                    Debug.Log("Knots belong to the same spline. Not linking.");
                }
            }
            else
            {
                Debug.LogWarning("No KnotMover found on the collided object.");
            }
        }
    }
}

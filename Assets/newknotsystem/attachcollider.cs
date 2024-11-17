using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineKnotLogger : MonoBehaviour
{
    public SplineContainer splineContainer; // Reference to the spline container
    public void disablemeshandactivate()
    {
        DisableMeshRendererAndRigidbody(); // Call the function to disable components

        if (splineContainer != null)
        {
            // Get all KnotMover components in child GameObjects
            KnotMover[] knotMovers = GetComponentsInChildren<KnotMover>();

            // Loop through each spline in the container
            for (int i = 0; i < splineContainer.Splines.Count; i++)
            {
                Spline spline = splineContainer.Splines[i];

                // Get the index of the last knot in the spline
                int lastKnotIndex = spline.Count - 1;

                // Check if the spline has knots
                if (lastKnotIndex >= 0)
                {
                    Debug.Log("Spline " + i + " has a last knot index of: " + lastKnotIndex);

                    // Iterate through all KnotMover components
                    foreach (KnotMover knotMover in knotMovers)
                    {
                        // Check if the KnotMover corresponds to the last knot of the current spline
                        if (knotMover.splineIndex == i && knotMover.knotIndex == lastKnotIndex)
                        {
                            // Activate the child GameObject of the last knot
                            if (knotMover.transform.childCount > 0)
                            {
                                knotMover.transform.GetChild(0).gameObject.SetActive(true);
                                Debug.Log("Activated child GameObject of last knot in spline " + i);
                            }
                        }
                        else if (knotMover.splineIndex == i)
                        {
                            // Deactivate the child GameObject for other knots within the same spline
                            if (knotMover.transform.childCount > 0)
                            {
                                knotMover.transform.GetChild(0).gameObject.SetActive(false);
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("Spline " + i + " has no knots.");
                }
            }
        }
        else
        {
            Debug.Log("Spline container is not assigned.");
        }
    }
    

    // Function to disable MeshRenderer and Rigidbody components on all child GameObjects
    void DisableMeshRendererAndRigidbody()
    {
        // Get all child GameObjects
        foreach (Transform child in transform)
        {
            // Disable the MeshRenderer component if it exists
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }

            // Disable the Rigidbody component if it exists
            /*Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // Set Rigidbody to kinematic to disable physics simulation
                rb.detectCollisions = false; // Optionally disable collision detection
            }*/
        }

        Debug.Log("Disabled MeshRenderer and Rigidbody components on all child GameObjects.");
    }
}

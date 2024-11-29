using UnityEngine;
using UnityEngine.Splines;

public class SplineLinker : MonoBehaviour
{
    private KnotMover knotMover;
    private GameObject cube;
    private SplineDrawer splineDrawer;
    private SplineInfo splineinfo;
    private void OnEnable()
    {
        knotMover = GetComponent<KnotMover>();
        // Assuming this script is attached to a child GameObject of the parent with SplineInfo
        splineinfo = transform.parent.GetComponent<SplineInfo>();

        // Check if KnotMover was found
        if (knotMover == null)
        {
            Debug.LogError("KnotMover component not found on this GameObject!");
            return;
        }

        // Access the cube reference from KnotMover
        cube = knotMover.cube;
        if (splineDrawer != null)
        {
            splineDrawer.splinesetagain();
            Debug.Log("Called splinesetagain on the SplineDrawer component after joining splines.");
        }
        // Check if cube is assigned
        if (cube == null)
        {
            Debug.LogError("Cube GameObject is not assigned in KnotMover or not found in the scene.");
        }
        
    }



    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object has the "knot" tag
        if (other.CompareTag("knot"))
        {
            //Debug.Log("Checking if knots can be linked");

           
          

            // Get the KnotMover component from the other GameObject
            KnotMover otherKnotMover = other.GetComponent<KnotMover>();
            if (otherKnotMover != null && knotMover !=null)
            {
                // Access the SplineContainer and spline indices from both objects
                SplineContainer currentContainer = knotMover.splineContainer;
                if (currentContainer!=null)
                {
                    // Save the necessary data before destruction
                    SplineKnotIndex knotA = new SplineKnotIndex(knotMover.splineIndex, knotMover.knotIndex);
                    SplineKnotIndex knotB = new SplineKnotIndex(otherKnotMover.splineIndex, otherKnotMover.knotIndex);

                    // Only link if the knots are on different splines
                    if (knotA.Spline != knotB.Spline)
                    {
                        splineinfo?.SetBeforeJoinBoolFalse();

                        currentContainer.JoinSplinesOnKnots(knotA, knotB);
                        //Debug.Log("The gameobject being handled is now destroyed.");
                        splineinfo?.ReinitializeKnots(this.gameObject);
                        Destroy(gameObject);
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




}
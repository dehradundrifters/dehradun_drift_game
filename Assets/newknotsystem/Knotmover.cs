using UnityEngine;
using UnityEngine.Splines;

public class KnotMover : MonoBehaviour
{
    private int knotIndex;
    private SplineContainer splineContainer;
    private Spline spline;
    private GameObject cube; // Reference for the cube

    // Initialize the sphere with the knot index and spline container reference
    public void Initialize(int knotIndex, SplineContainer container)
    {
        this.knotIndex = knotIndex;
        this.splineContainer = container;
        spline = splineContainer.Spline;

        // Find the cube in the scene
        cube = GameObject.FindWithTag("Cube"); // Ensure the cube has the tag "Cube"

        if (cube == null)
        {
            Debug.LogError("Cube not found in the scene! Please make sure the cube is present and tagged correctly.");
        }
    }

    void Update()
    {
        if (cube == null) return; // Early return if cube is not found

        // Check if this sphere corresponds to the last knot
        if (knotIndex == spline.Count - 1)
        {
            // Get the cube's position in local space of the splineContainer
            Vector3 cubeLocalPosition = splineContainer.transform.InverseTransformPoint(cube.transform.position);

            // Set both the knot and the sphere to the cube's position
            transform.position = cube.transform.position; // Move the sphere to cube position
            BezierKnot newKnot = new BezierKnot(cubeLocalPosition);

            // Update the last knot to match the cube's position
            spline[knotIndex] = newKnot;

            // Ensure tangent mode is autosmooth for the last knot
            spline.SetTangentMode(knotIndex, TangentMode.AutoSmooth);
        }
        else
        {
            // Sync the knot's position with the object's position (for non-last knots)
            Vector3 localPosition = splineContainer.transform.InverseTransformPoint(transform.position);
            BezierKnot currentKnot = spline[knotIndex];
            BezierKnot newKnot = new BezierKnot(localPosition, currentKnot.TangentIn, currentKnot.TangentOut);

            spline[knotIndex] = newKnot;
            spline.SetTangentMode(knotIndex, TangentMode.AutoSmooth);
        }
    }
}

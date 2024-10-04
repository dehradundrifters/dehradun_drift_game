using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class KnotMover : MonoBehaviour
{
    public int knotIndex;
    public SplineContainer splineContainer;
    public Spline spline;
    public int splineIndex;
    public GameObject cube;
    public bool isCursorOverlapping = false;
    private ToggleReference toggleReference;
    public InputActionProperty leftgripAction;
    public void Initialize(int knotIndex, SplineContainer container, int splineIndex)
    {
        this.knotIndex = knotIndex;
        this.splineContainer = container;
        this.splineIndex = splineIndex;
        spline = container.Splines[splineIndex];

        cube = GameObject.FindWithTag("Cube");
        if (cube == null)
        {
            Debug.LogError("Cube not found in the scene! Please make sure the cube is present and tagged correctly.");
        }
        toggleReference = gameObject.GetComponent<ToggleReference>();
        if (toggleReference == null)
        {
            Debug.LogError("ToggleReference script not found! Make sure it is attached and assigned correctly.");
        }
    }

    void Update()
    {
        if (cube == null) return;

        // Get the currently active spline (the last spline in the SplineContainer)
        Spline activeSpline = splineContainer.Splines[splineContainer.Splines.Count - 1];

        // Get the spline corresponding to our splineIndex
        Spline currentSpline = splineContainer.Splines[splineIndex];

        // Check if this is the last knot of the active spline
        if (currentSpline == activeSpline && knotIndex == activeSpline.Count - 1)
        {
            // Move the last knot of the active spline to the cube's position
            Vector3 cubeLocalPosition = splineContainer.transform.InverseTransformPoint(cube.transform.position);
            transform.position = cube.transform.position;
            BezierKnot newKnot = new BezierKnot(cubeLocalPosition);
            activeSpline[knotIndex] = newKnot;
            activeSpline.SetTangentMode(knotIndex, TangentMode.AutoSmooth);
        }
        else
        {
            // For all other knots
            Vector3 localPosition = splineContainer.transform.InverseTransformPoint(transform.position);
            BezierKnot currentKnot = currentSpline[knotIndex];
            BezierKnot newKnot = new BezierKnot(localPosition, currentKnot.TangentIn, currentKnot.TangentOut);
            currentSpline[knotIndex] = newKnot;
            currentSpline.SetTangentMode(knotIndex, TangentMode.AutoSmooth);
        }

        // Check for cursor overlap and T key press
        if (toggleReference.deleteKnotToggle.isOn && isCursorOverlapping && knotIndex == currentSpline.Count - 1)
        {
            Debug.Log("hovering on last knot");
            if (leftgripAction.action.WasPressedThisFrame())
            {
                DeleteKnot();
            }
            Debug.Log($"This is the last knot of spline {splineIndex}.");
        }
    }

    void DeleteKnot()
    {
        // Check if the current spline and knot index are valid
        Spline currentSpline = splineContainer.Splines[splineIndex];
        if (currentSpline != null && knotIndex >= 0 && knotIndex < currentSpline.Count)
        {
            // Remove the last knot from the specific spline
            currentSpline.RemoveAt(knotIndex);
            Debug.Log($"Knot at index {knotIndex} of spline {splineIndex} removed.");

            // Set the tangent mode for the new last knot if needed
            if (currentSpline.Count > 0)
            {
                int newLastKnotIndex = currentSpline.Count - 1;
                currentSpline.SetTangentMode(newLastKnotIndex, TangentMode.AutoSmooth);
            }

            // Destroy the GameObject representing this knot
            Destroy(gameObject);
            Debug.Log($"GameObject of the knot at index {knotIndex} destroyed.");
        }
        else
        {
            Debug.LogWarning("Invalid knot index or spline. Unable to delete knot.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            isCursorOverlapping = true;
            Debug.Log("Cursor overlap");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            isCursorOverlapping = false;
        }
    }
}

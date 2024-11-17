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
    public bool beforejoinbool = true;
    public InputActionReference leftTriggerAction; // Reference for the left trigger button

    private void Start()
    {
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

    private void OnEnable()
    {
        leftTriggerAction.action.Enable();
    }

    private void OnDisable()
    {
        leftTriggerAction.action.Disable();
    }

    public void Initialize(int knotIndex, SplineContainer container, int splineIndex)
    {
        this.knotIndex = knotIndex;
        this.splineContainer = container;
        this.splineIndex = splineIndex;
        spline = container.Splines[splineIndex];

        // Get the knot's current position in world space
        if (knotIndex >= 0 && knotIndex < spline.Count)
        {
            Vector3 knotWorldPosition = splineContainer.transform.TransformPoint(spline[knotIndex].Position);
            transform.position = knotWorldPosition;
        }
        beforejoinbool = true;
    }

    void Update()
    {
        if (cube == null) return;

        // Get the currently active spline (the last spline in the SplineContainer)
        Spline activeSpline = splineContainer.Splines[splineContainer.Splines.Count - 1];

        // Get the spline corresponding to our splineIndex
        Spline currentSpline = splineContainer.Splines[splineIndex];
        if (beforejoinbool)
        {
            if (splineIndex == splineContainer.Splines.Count - 1 && knotIndex == spline.Count - 1)
            {
                Vector3 cubeLocalPosition = splineContainer.transform.InverseTransformPoint(cube.transform.position);
                transform.position = cube.transform.position;
                BezierKnot newKnot = new BezierKnot(cubeLocalPosition);
                spline[knotIndex] = newKnot;
                spline.SetTangentMode(knotIndex, TangentMode.AutoSmooth);
            }
            else
            {
                Vector3 localPosition = splineContainer.transform.InverseTransformPoint(transform.position);
                BezierKnot currentKnot = currentSpline[knotIndex];
                BezierKnot newKnot = new BezierKnot(localPosition, currentKnot.TangentIn, currentKnot.TangentOut);
                currentSpline[knotIndex] = newKnot;
                currentSpline.SetTangentMode(knotIndex, TangentMode.AutoSmooth);
            }
        }

        // Check for cursor overlap and deletion toggle
        if (toggleReference.deleteKnotToggle.isOn && isCursorOverlapping)
        {
            if (leftTriggerAction.action.triggered) // Check if the left trigger action is triggered
            {
                DeleteKnot();
            }
            Debug.Log($"Knot at index {knotIndex} of spline {splineIndex} can be deleted.");
        }
    }

    // Method to delete the current knot
    public void DeleteKnot()
    {
        Spline currentSpline = splineContainer.Splines[splineIndex];

        // Validate knot index and spline
        if (currentSpline != null && knotIndex >= 0 && knotIndex < currentSpline.Count)
        {
            currentSpline.RemoveAt(knotIndex);
            Debug.Log($"Knot at index {knotIndex} of spline {splineIndex} removed.");

            if (currentSpline.Count > 0)
            {
                int newLastKnotIndex = currentSpline.Count - 1;
                currentSpline.SetTangentMode(newLastKnotIndex, TangentMode.AutoSmooth);
            }

            SplineInfo splineInfo = GetComponentInParent<SplineInfo>();
            if (splineInfo != null)
            {
                splineInfo.ReinitializeKnots(this.gameObject);
                Debug.Log("Called ReinitializeKnots on the parent SplineInfo, ignoring this GameObject.");
            }

            Destroy(gameObject);
            Debug.Log($"GameObject of the knot at index {knotIndex} destroyed.");
        }
        else
        {
           // Debug.LogWarning("Invalid knot index or spline. Unable to delete knot.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            isCursorOverlapping = true;
            //Debug.Log("Cursor overlap");
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

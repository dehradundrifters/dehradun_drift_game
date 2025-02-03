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
    
    public bool beforejoinbool = true;
    
    


    private void Start()
    {
        cube = GameObject.FindWithTag("Cube");
        if (cube == null)
        {
            Debug.LogError("Cube not found in the scene! Please make sure the cube is present and tagged correctly.");
        }
       
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
                Quaternion cubeLocalRotation = Quaternion.Inverse(splineContainer.transform.rotation) * cube.transform.rotation;

                transform.position = cube.transform.position;
                transform.rotation = cube.transform.rotation;

                BezierKnot newKnot = new BezierKnot(cubeLocalPosition, float3.zero, float3.zero, cubeLocalRotation);
                spline[knotIndex] = newKnot;
                spline.SetTangentMode(knotIndex, TangentMode.AutoSmooth);
            }
            else
            {
                Vector3 localPosition = splineContainer.transform.InverseTransformPoint(transform.position);
                Quaternion localRotation = Quaternion.Inverse(splineContainer.transform.rotation) * transform.rotation;

                BezierKnot currentKnot = currentSpline[knotIndex];
                BezierKnot newKnot = new BezierKnot(localPosition, currentKnot.TangentIn, currentKnot.TangentOut, localRotation);
                currentSpline[knotIndex] = newKnot;
                currentSpline.SetTangentMode(knotIndex, TangentMode.AutoSmooth);
            }
        }

        // Check for cursor overlap and deletion toggle
        if (isCursorOverlapping && InputManager.Instance.IsDeleteEnabled && InputManager.Instance.IsTriggerPressed)
        {
            Debug.Log($"Deleting GameObject: {gameObject.name}");
            DeleteKnot(); // Delete this GameObject
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
        // Check if the hand enters the trigger area
        if (other.CompareTag("Hand"))
        {
            isCursorOverlapping = true;
            Debug.Log($"Cursor overlapping {gameObject.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the hand exits the trigger area
        if (other.CompareTag("Hand"))
        {
            isCursorOverlapping = false;
            Debug.Log($"Cursor left {gameObject.name}");
        }
    }
}

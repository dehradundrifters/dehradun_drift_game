using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class SplineDrawer : MonoBehaviour
{
    public SplineContainer splineContainer; // Assign your SplineContainer in the inspector
    public GameObject objectToInstantiate;  // Assign the prefab to instantiate in the inspector
    private Spline spline;
    private bool knotIsFollowingCube = true; // To check if the current knot should move with the cube
    private int currentKnotIndex = -1; // Track the index of the current knot
    public InputActionProperty rightTriggerAction;
    void Start()
    {
        // Get the spline from the SplineContainer
        spline = splineContainer.Spline;

        // Add the first knot at the cube's initial position (the position of this GameObject)
        AddNewKnotAtPosition(transform.position);
    }

    void Update()
    {
        // If the spacebar is pressed, finalize the current knot and create a new one
        if (rightTriggerAction.action.WasPressedThisFrame())
        {
            // Stop the current knot from following the cube
            knotIsFollowingCube = false;

            // Add a new knot and make it follow the cube
            AddNewKnotAtPosition(transform.position);

            // Instantiate an object at the position of the new knot and assign its knot index
            InstantiateAtKnotPosition();
        }

        // Move the current knot with the cube while the knot is set to follow the cube
        if (knotIsFollowingCube && currentKnotIndex >= 0)
        {
            // Convert the cube's world position to local position relative to the SplineContainer
            Vector3 localPosition = splineContainer.transform.InverseTransformPoint(transform.position);

            // Update the current knot position to follow the cube
            spline[currentKnotIndex] = new BezierKnot(localPosition);
        }
    }

    void AddNewKnotAtPosition(Vector3 position)
    {
        // Convert the cube's world position to local position relative to the SplineContainer
        Vector3 localPosition = splineContainer.transform.InverseTransformPoint(position);

        // Add a new spline knot at the local position
        BezierKnot newKnot = new BezierKnot(localPosition);
        spline.Add(newKnot);

        // Set the tangent mode to AutoSmooth
        spline.SetTangentMode(spline.Count - 1, TangentMode.AutoSmooth);

        // Set the new knot to follow the cube
        currentKnotIndex = spline.Count - 1;
        knotIsFollowingCube = true;
    }

    void InstantiateAtKnotPosition()
    {
        // Get the current knot's world position
        Vector3 knotWorldPosition = splineContainer.transform.TransformPoint(spline[currentKnotIndex].Position);

        // Instantiate the object at the knot's world position
        GameObject newObject = Instantiate(objectToInstantiate, knotWorldPosition, Quaternion.identity);

        // Assign the knot index to the instantiated object
        newObject.GetComponent<KnotMover>().Initialize(currentKnotIndex, splineContainer);
    }
}

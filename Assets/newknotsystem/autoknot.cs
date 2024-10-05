



using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class SplineDrawer : MonoBehaviour
{
    public SplineContainer splineContainer; // The SplineContainer that holds all splines
    public GameObject objectToInstantiate;  // The prefab to instantiate at each knot
    private Spline spline;
    private bool knotIsFollowingCube = true; // To check if the current knot should move with the cube
    private int currentKnotIndex = -1; // Track the index of the current knot
    private int currentSplineIndex = 0; // Track the index of the current spline within the container
    private GameObject[] instantiatedObjects; // Track instantiated objects for the current spline
    private bool canCreateKnots = true; // To track if the toggle is on
    public InputActionProperty rightTriggerAction;
    
    void Start()
    {
        // Initialize the object tracking array
        instantiatedObjects = new GameObject[100]; // Adjust size as needed

        // Get the first spline from the SplineContainer
        if (splineContainer.Splines.Count > 0)
        {
            spline = splineContainer.Splines[currentSplineIndex];
        }
        else
        {
            // Create the first spline if none exist
            spline = new Spline();
            splineContainer.AddSpline(spline);
        }

        // Add the first knot at the cube's initial position (the position of this GameObject)
        AddNewKnotAtPosition(transform.position);

        // Instantiate the object at the position of the first knot
        InstantiateAtKnotPosition();
    }
    public void SetKnotCreationState(bool state)
    {
        canCreateKnots = state;

    }
    void Update()
    {
        // If the spacebar is pressed, finalize the current knot and create a new one
        if (rightTriggerAction.action.WasPressedThisFrame() && canCreateKnots)
        {
            // Stop the current knot from following the cube
            knotIsFollowingCube = false;

            // Add a new knot and make it follow the cube
            AddNewKnotAtPosition(transform.position);

            // Instantiate an object at the position of the new knot
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

        // Switch to a new spline when N is pressed
        if (Input.GetKeyDown(KeyCode.N)) 
        {
            SwitchToNewSpline();
        }
    }

    // Adds a new knot at the given position and updates the spline
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

    // Instantiates an object at the current knot position and links it with the knot
    void InstantiateAtKnotPosition()
    {
        // Get the current knot's world position
        Vector3 knotWorldPosition = splineContainer.transform.TransformPoint(spline[currentKnotIndex].Position);

        // Instantiate the object at the knot's world position
        GameObject newObject = Instantiate(objectToInstantiate, knotWorldPosition, Quaternion.identity);

        // Store reference to instantiated object
        instantiatedObjects[currentKnotIndex] = newObject;

        // Assign the knot index and SplineContainer to the new object using the KnotMover script
        KnotMover knotMover = newObject.GetComponent<KnotMover>();
        if (knotMover != null)
        {
            knotMover.Initialize(currentKnotIndex, splineContainer, currentSplineIndex);
        }
        else
        {
            Debug.LogError("KnotMover component not found on the instantiated object.");
        }
    }

    // Switches to a new spline inside the same SplineContainer
    void SwitchToNewSpline()
    {
        // Preserve the current spline as is, and create a completely new spline
        spline = new Spline();
        splineContainer.AddSpline(spline);
        currentSplineIndex = splineContainer.Splines.Count - 1; // Update to the new spline's index

        // Reset the knot tracking variables for the new spline
        currentKnotIndex = -1;
        knotIsFollowingCube = true;

        // Clear the array tracking objects to avoid transferring objects from the old spline
        instantiatedObjects = new GameObject[100]; // Start fresh with new objects for the new spline

        // Add the first knot at the cube's current position
        AddNewKnotAtPosition(transform.position);

        // Instantiate an object at the position of the first knot on the new spline
        InstantiateAtKnotPosition();

        Debug.Log("Switched to a new spline.");
    }
}
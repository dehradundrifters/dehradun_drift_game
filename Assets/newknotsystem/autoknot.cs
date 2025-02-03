using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using UnityEngine.UI;

public class SplineDrawer : MonoBehaviour
{
    public SplineContainer splineContainer; // The SplineContainer that holds all splines
    public GameObject objectToInstantiate;  // The prefab to instantiate at each knot
    public GameObject parentObject; // The parent object to assign to the instantiated objects
    public InputActionReference rightTriggerAction; // Reference for the right trigger button
    public InputActionReference aButtonAction; // Reference for the A button on VR controller
    public bool notworkingOnSpline = false;
    private Spline spline;
    private bool knotIsFollowingCube = true; // To check if the current knot should move with the cube
    private int currentKnotIndex = -1; // Track the index of the current knot
    private int currentSplineIndex = 0; // Track the index of the current spline within the container
    private GameObject[] instantiatedObjects; // Track instantiated objects for the current spline
    public bool canCreateKnots = true; // To track if knot creation is enabled (controlled by UI Toggle)
    public bool invalid = false;
    //public Toggle knotcreate;
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

        // Add the first knot at the GameObject's initial position
        AddNewKnotAtPosition(transform.position);

        // Instantiate the object at the position of the first knot
        InstantiateAtKnotPosition();
    }

    private void OnEnable()
    {
        // Enable VR input actions and set up event listeners
        rightTriggerAction.action.performed += OnRightTriggerPressed;
        aButtonAction.action.performed += OnAButtonPressed;
        rightTriggerAction.action.Enable();
        aButtonAction.action.Enable();
    }

    private void OnDisable()
    {
        // Remove event listeners and disable VR input actions
        rightTriggerAction.action.performed -= OnRightTriggerPressed;
        aButtonAction.action.performed -= OnAButtonPressed;
        rightTriggerAction.action.Disable();
        aButtonAction.action.Disable();
    }

    private void OnRightTriggerPressed(InputAction.CallbackContext context)
    {
        // Check if knot creation is enabled
        if (!invalid && canCreateKnots)
        {
            knotIsFollowingCube = false;
            AddNewKnotAtPosition(transform.position);
            InstantiateAtKnotPosition();

            // Set workingOnSpline to false after creating a knot
            notworkingOnSpline = false;
        }
    }

    private void OnAButtonPressed(InputAction.CallbackContext context)
    {
        if (!invalid)
        {
            SwitchToNewSpline();

            // Set workingOnSpline to true when switching to a new spline
            notworkingOnSpline = true;
        }
    }

    void Update()
    {
        // Move the current knot with the cube while the knot is set to follow the cube
        if (knotIsFollowingCube && currentKnotIndex >= 0)
        {
            Vector3 localPosition = splineContainer.transform.InverseTransformPoint(transform.position);
            spline[currentKnotIndex] = new BezierKnot(localPosition);
        }
    }

    // Adds a new knot at the given position and updates the spline
    void AddNewKnotAtPosition(Vector3 position)
    {
        Vector3 localPosition = splineContainer.transform.InverseTransformPoint(position);
        BezierKnot newKnot = new BezierKnot(localPosition);
        spline.Add(newKnot);
        spline.SetTangentMode(spline.Count - 1, TangentMode.AutoSmooth);

        currentKnotIndex = spline.Count - 1;
        knotIsFollowingCube = true;
    }

    // Instantiates an object at the current knot position and links it with the knot
    void InstantiateAtKnotPosition()
    {
        Vector3 knotWorldPosition = splineContainer.transform.TransformPoint(spline[currentKnotIndex].Position);
        GameObject newObject = Instantiate(objectToInstantiate, knotWorldPosition, Quaternion.identity);

        if (parentObject != null)
        {
            newObject.transform.SetParent(parentObject.transform);
        }
        else
        {
            Debug.LogWarning("Parent object is not assigned. Instantiated object will not have a parent.");
        }

        instantiatedObjects[currentKnotIndex] = newObject;

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
        spline = new Spline();
        splineContainer.AddSpline(spline);
        currentSplineIndex = splineContainer.Splines.Count - 1;

        currentKnotIndex = -1;
        knotIsFollowingCube = true;
        instantiatedObjects = new GameObject[100];

        AddNewKnotAtPosition(transform.position);
        InstantiateAtKnotPosition();

        Debug.Log("Switched to a new spline.");
    }

    // Method to enable or disable knot creation, linked to the UI Toggle
    public void SetKnotCreationState(bool state)
    {
        canCreateKnots = state;
        Debug.Log(" toggle state" + canCreateKnots);
        splinesetagain();
        if (!state && !notworkingOnSpline )
        {
            SwitchToNewSpline();
        }
    }

    // Sets or creates a new spline based on the current toggle state
    public void splinesetagain()
    {
        if (canCreateKnots)
        {
            // Update currentSplineIndex based on the number of splines in the container
            if (splineContainer.Splines.Count > 0)
            {
                currentSplineIndex = splineContainer.Splines.Count - 1;
            }
            else
            {
                // If no splines exist, create a new spline
                spline = new Spline();
                splineContainer.AddSpline(spline);
                currentSplineIndex = 0;
            }

            // Update the spline reference to the newly selected or created spline
            spline = splineContainer.Splines[currentSplineIndex];
            Debug.Log("Knot creation enabled. Current Spline Index: " + currentSplineIndex);
        }
    }
}

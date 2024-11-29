using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class splitknot : MonoBehaviour
{
    public Toggle split;
    public SplineContainer splineContainer;
    [SerializeField] public GameObject knotparent;
    [SerializeField] public GameObject knotpointPrefab;
    [SerializeField] private SplineInfo splineInfo;
    public InputActionReference rightTriggerAction; // Reference for the right trigger button

    private Spline spline;
    private int knotIndex;
    private int splineIndex;
    private bool isCollidingWithKnot = false;

    private void Start()
    {
        splineInfo = knotparent.GetComponent<SplineInfo>();
    }

    private void OnEnable()
    {
        // Enable the right trigger action
        rightTriggerAction.action.Enable();
    }

    private void OnDisable()
    {
        // Disable the right trigger action
        rightTriggerAction.action.Disable();
    }

    void Update()
    {
        // Check if colliding with knot, split toggle is on, and right trigger is pressed
        if (isCollidingWithKnot && split.isOn && rightTriggerAction.action.triggered)
        {
            splittheknot();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("knot"))
        {
            KnotMover knotMover = other.GetComponent<KnotMover>();
            if (knotMover != null)
            {
                knotIndex = knotMover.knotIndex;
                splineIndex = knotMover.splineIndex;
                isCollidingWithKnot = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("knot"))
        {
            isCollidingWithKnot = false;
        }
    }

    public void splittheknot()
    {
        // Set beforejoinbool to false for all child knots
        splineInfo.SetBeforeJoinBoolFalse();

        // Define the knot where the split will occur
        SplineKnotIndex knot = new SplineKnotIndex(splineIndex, knotIndex);

        // Remove the last spline from the container
        int lastSplineIndex = splineContainer.Splines.Count - 1;
        if (lastSplineIndex >= 0)
        {
            splineContainer.RemoveSplineAt(lastSplineIndex);
        }

        // Split the current spline on the specified knot
        splineContainer.SplitSplineOnKnot(knot);

        // Create a completely new spline and add it to the SplineContainer
        Spline newSpline = new Spline();
        splineContainer.AddSpline(newSpline);

        // Convert the current position to the local space of the SplineContainer
        Vector3 localPosition = splineContainer.transform.InverseTransformPoint(transform.position);

        // Add the first knot at the new spline with the current local position
        BezierKnot newKnot = new BezierKnot(localPosition);
        newSpline.Add(newKnot);
        newSpline.SetTangentMode(0, TangentMode.AutoSmooth); // Set to AutoSmooth

        // Instantiate the knotpointPrefab at the position of the new knot and assign it to the parent
        GameObject newKnotpoint = Instantiate(knotpointPrefab, transform.position, Quaternion.identity, knotparent.transform);


        splineInfo.onlyreinitialize();
    }
}

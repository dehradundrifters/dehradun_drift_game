using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.InputSystem; // For the new Input System

public class ControlPointAdder : MonoBehaviour
{
    public SplineContainer splineContainer;
    public GameObject cube;
    public GameObject tangentPrefab;
    public InputActionProperty rightTriggerAction; // Reference to the right-hand trigger action

    void Start()
    {
        Spline spline = splineContainer.Spline;

        // Optionally, add an initial knot
        AddKnotToSpline(spline, cube.transform.position);
    }

    void AddKnotToSpline(Spline spline, Vector3 position)
    {
        Vector3 tangentIn = new Vector3(-1, 0, 0);
        Vector3 tangentOut = new Vector3(1, 0, 0);

        BezierKnot newKnot = new BezierKnot((float3)position, (float3)tangentIn, (float3)tangentOut);

        spline.Add(newKnot);

        int knotIndex = spline.Count - 1;

        GameObject tangentInObject = Instantiate(tangentPrefab, position + tangentIn, Quaternion.identity);
        GameObject tangentOutObject = Instantiate(tangentPrefab, position + tangentOut, Quaternion.identity);

        tangentInObject.AddComponent<TangentHandle>().Initialize(spline, knotIndex, true);
        tangentOutObject.AddComponent<TangentHandle>().Initialize(spline, knotIndex, false);
    }

    void Update()
    {
        // Check for right trigger press (using Input System)
        if (rightTriggerAction.action.WasPressedThisFrame())
        {
            AddKnotToSpline(splineContainer.Spline, cube.transform.position);
            Debug.Log("Right trigger pressed, adding knot to spline.");
        }
    }
}

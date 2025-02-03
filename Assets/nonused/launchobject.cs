using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

[RequireComponent(typeof(SplineAnimate))]
[RequireComponent(typeof(Rigidbody))]
public class SplineLaunch : MonoBehaviour
{
    private SplineAnimate splineAnimate;
    private Rigidbody rb;
    private float splineLength;
    private bool launched = false;
    void Start()
    {
        splineAnimate = GetComponent<SplineAnimate>();
        rb = GetComponent<Rigidbody>();
                // Use built-in function to calculate the total length of the spline with the object's local-to-world transform
        splineLength = SplineUtility.CalculateLength(splineAnimate.splineContainer.Spline, transform.localToWorldMatrix);
    }

    void Update()
    {
        // Check if the object has reached the end of the spline
        if (splineAnimate.NormalizedTime >= 1f)
        {
            if (!launched)
            {
                LaunchObject();
                launched = true;
            }
           
        }
    }

    void LaunchObject()
    {
        // Disable Spline Animate component
        splineAnimate.enabled = false;

        // Get the current direction of the object at the end of the spline
        Vector3 direction = GetForwardDirectionAtEnd();

        // Get the duration from the SplineAnimate component
        float launchDuration = splineAnimate.duration;

        // Calculate speed as distance covered divided by time taken
        float speed = splineLength / launchDuration;

        // Apply force based on speed in the direction it was moving
        rb.AddForce(direction * speed, ForceMode.Impulse);
    }

    Vector3 GetForwardDirectionAtEnd()
    {
        // Get the spline from the SplineAnimate component
        Spline spline = splineAnimate.splineContainer.Spline;

        // Evaluate the tangent at the end of the spline (last knot)
        float lastKnotTime = spline.Count - 1; // Time at the last knot

        // Use Unity.Mathematics to evaluate and normalize the float3 tangent
        float3 tangent = spline.EvaluateTangent(lastKnotTime);
        float3 normalizedTangent = math.normalize(tangent); // Normalize the tangent

        // Convert float3 to Vector3 (required by Rigidbody.AddForce)
        return new Vector3(normalizedTangent.x, normalizedTangent.y, normalizedTangent.z);
    }
}

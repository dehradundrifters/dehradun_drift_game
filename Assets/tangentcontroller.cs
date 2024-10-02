using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics; // Ensure this is included

public class TangentHandle : MonoBehaviour
{
    private Spline spline;
    private int knotIndex;
    private bool isTangentIn;

    
    public void Initialize(Spline spline, int knotIndex, bool isTangentIn)
    {
        this.spline = spline;
        this.knotIndex = knotIndex;
        this.isTangentIn = isTangentIn;
    }

    void Update()
    {
        if (isTangentIn)
        {
            BezierKnot knot = spline[knotIndex];
            knot.TangentIn = (float3)transform.position - knot.Position;
            spline[knotIndex] = knot;
        }
        else
        {
            BezierKnot knot = spline[knotIndex];
            knot.TangentOut = (float3)transform.position - knot.Position;
            spline[knotIndex] = knot;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}

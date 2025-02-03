using UnityEngine;
using Unity.XR.CoreUtils;

public class LookHandle : MonoBehaviour
{
    public XROrigin xrOrigin;        // The XR Origin to reposition and rotate
    public Transform handle;         // The handle to face towards
    public Transform resetPos;       // The GameObject defining the reset position for the XR Origin

    void OnEnable()
    {
        // Ensure required references are assigned
        if (xrOrigin == null || handle == null || resetPos == null)
        {
            Debug.LogError("XR Origin, Handle, or Reset Position is not assigned!");
            return;
        }

        // Align the XR Origin's position and rotation
        AlignXROrigin();
    }

    private void AlignXROrigin()
    {
        // Move the XR Origin to the reset position
        xrOrigin.transform.position = resetPos.position;

        // Calculate the direction to look at the handle
        Vector3 directionToHandle = handle.position - xrOrigin.transform.position;

        // Ignore the vertical component (Y-axis) of the direction
        directionToHandle.y = 0f;
        directionToHandle.Normalize();

        // Create a new rotation that only modifies the Y-axis
        Quaternion targetRotation = Quaternion.LookRotation(directionToHandle, Vector3.up);

        // Apply the new rotation to the XR Origin
        xrOrigin.transform.rotation = targetRotation;

        Debug.Log($"{xrOrigin.name} moved to {resetPos.name}, and is now facing {handle.name} on the Y-axis only.");
    }
}
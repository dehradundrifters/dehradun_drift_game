using UnityEngine;

public class CylinderMaterialChanger : MonoBehaviour
{
    public Material normalMaterial; // The normal/default material of the cylinder
    public Material invalidMaterial; // The material to switch to when in contact with the "track"
    public SplineDrawer splinedraw;

    private Renderer cylinderRenderer;
    private bool isInContact = false; // Tracks if the cylinder is in contact with the "track"
    private bool currentState = false; // Tracks the current collision state

    void Start()
    {
        cylinderRenderer = GetComponent<Renderer>();

        if (normalMaterial != null)
        {
            cylinderRenderer.material = normalMaterial;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("track"))
        {
            // Mark as in contact
            isInContact = true;

            if (!currentState)
            {
                // Switch to invalid material if the state has changed
                splinedraw.invalid = true;
                cylinderRenderer.material = invalidMaterial;
                currentState = true;
            }
        }
    }

    private void Update()
    {
        if (!isInContact && currentState)
        {
            // If not in contact and the state has changed, reset the material
            splinedraw.invalid = false;
            cylinderRenderer.material = normalMaterial;
            currentState = false;
        }

        // Reset `isInContact` at the end of each frame
        isInContact = false;
    }
}

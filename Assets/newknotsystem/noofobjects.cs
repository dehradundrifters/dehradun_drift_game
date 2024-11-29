using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Splines; // Ensure this namespace is included for SplineContainer

public class KnotToggleController : MonoBehaviour
{
    [Header("Settings")]
    public Toggle toggle; // Public toggle UI element
    public int maxNumberOfKnots = 40; // Maximum allowed knots
    public SplineContainer splineContainer; // The container of the splines
    public SplineDrawer splineDrawer; // Reference to SplineDrawer script

    private void Update()
    {
        if (splineContainer == null || toggle == null || splineDrawer == null)
        {
            Debug.LogWarning("SplineContainer, Toggle, or SplineDrawer is not assigned!");
            return;
        }

        int totalKnots = GetTotalKnotCount();

        // Check if the total knots exceed the limit
        if (totalKnots > maxNumberOfKnots)
        {
            if (toggle.isOn)
            {
                toggle.isOn = false; // Turn off the toggle
            }
            toggle.interactable = false; // Disable interaction
            splineDrawer.canCreateKnots = false; // Disable knot creation
        }
        else
        {
            toggle.interactable = true; // Enable interaction
            splineDrawer.canCreateKnots = true; // Allow knot creation
        }
    }

    private int GetTotalKnotCount()
    {
        int totalKnots = 0;

        // Iterate through all splines in the container and sum up their knot counts
        foreach (var spline in splineContainer.Splines)
        {
            totalKnots += spline.Count; // Ensure 'Count' is valid for the spline type
        }

        return totalKnots;
    }
}

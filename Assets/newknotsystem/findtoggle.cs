using UnityEngine;
using UnityEngine.UI;

public class ToggleReference : MonoBehaviour
{
    // Variables to hold references to the toggles
    [SerializeField] public Toggle deleteKnotToggle;
    [SerializeField] public Toggle joinKnotToggle;

    void Start()
    {
        // Find the Canvas in the scene
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas != null)
        {
            // Try to find the DeleteKnot toggle under Canvas
            deleteKnotToggle = canvas.transform.Find("deleteknot")?.GetComponent<Toggle>();

            // Try to find the JoinKnot toggle under Canvas
            joinKnotToggle = canvas.transform.Find("joinknot")?.GetComponent<Toggle>();

            // Check if both toggles were found
            if (deleteKnotToggle == null || joinKnotToggle == null)
            {
                Debug.LogError("One or both toggles (DeleteKnot, JoinKnot) were not found under Canvas.");
            }
        }
        else
        {
            Debug.LogError("Canvas not found in the scene.");
        }
    }

    // Optional: Add any other functionality here to use the toggles
    void Update()
    {
        // Example: Log the toggle state for testing
        if (deleteKnotToggle != null && joinKnotToggle != null)
        {
            Debug.Log("Delete Knot Toggle: " + deleteKnotToggle.isOn);
            Debug.Log("Join Knot Toggle: " + joinKnotToggle.isOn);
        }
    }
}

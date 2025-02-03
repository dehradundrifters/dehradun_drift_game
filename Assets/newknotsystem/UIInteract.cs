using UnityEngine;
using UnityEngine.UI;

public class UIInteract : MonoBehaviour
{
    // Reference to the UI Toggle
    public Toggle uiToggle;

    // Called when another collider enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // Ensure the interacting object has the correct tag or component
        if (other.CompareTag("Hand")) // Replace "Hand" with the tag of your interacting object
        {
            // Toggle the state
            uiToggle.isOn = !uiToggle.isOn;
        }
    }

    // Called when another collider stays within the trigger zone
    private void OnTriggerStay(Collider other)
    {
        // Ensure the toggle remains active if it's already on
        if (other.CompareTag("Hand") && uiToggle.isOn)
        {
            // Optional: Additional behavior if needed
        }
    }

    // Called when another collider exits the trigger zone
    private void OnTriggerExit(Collider other)
    {
        // No additional action required upon exit for this behavior
    }
}

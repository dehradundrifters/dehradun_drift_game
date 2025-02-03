using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIButtonInteract : MonoBehaviour
{
    // Reference to the TextMeshProUGUI button
    public Button uiButton;

    // Optional color changes for visual feedback
    public TextMeshProUGUI buttonLabel;
    public Color defaultColor = Color.white;
    public Color hoverColor = Color.yellow;

    private void Start()
    {
        // Set the initial color of the button label
        if (buttonLabel != null)
        {
            buttonLabel.color = defaultColor;
        }
    }

    // Called when another collider enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand")) // Replace "Hand" with the tag of your interacting object
        {
            // Simulate button press by triggering its onClick functionality
            if (uiButton != null)
            {
                uiButton.onClick.Invoke();
            }

            // Change the button label color for hover feedback
            UpdateButtonVisual(hoverColor);
        }
    }

    // Called when another collider exits the trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            // Reset the button label color to its default state
            UpdateButtonVisual(defaultColor);
        }
    }

    // Update the button's visual state (e.g., color)
    private void UpdateButtonVisual(Color color)
    {
        if (buttonLabel != null)
        {
            buttonLabel.color = color;
        }
    }
}

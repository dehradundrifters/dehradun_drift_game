using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    public Toggle toggle1; // First toggle
    public Toggle toggle2; // Second toggle
    public Toggle toggle3; // Third toggle
    public Toggle toggle4; // Third toggle

    void Start()
    {
        // Add listener for each toggle to handle state changes
        toggle1.onValueChanged.AddListener(delegate { OnToggleChanged(toggle1); });
        toggle2.onValueChanged.AddListener(delegate { OnToggleChanged(toggle2); });
        toggle3.onValueChanged.AddListener(delegate { OnToggleChanged(toggle3); });
        toggle4.onValueChanged.AddListener(delegate { OnToggleChanged(toggle4); });
    }

    // This function will be called when any toggle changes its state
    void OnToggleChanged(Toggle changedToggle)
    {
        if (changedToggle.isOn)
        {
            // Turn off the other toggles
            if (changedToggle != toggle1) toggle1.isOn = false;
            if (changedToggle != toggle2) toggle2.isOn = false;
            if (changedToggle != toggle3) toggle3.isOn = false;
            if (changedToggle != toggle4) toggle4.isOn = false;
        }
    }

    // Optional: In case you need to reset all toggles at once
    public void ResetAllToggles()
    {
        toggle1.isOn = false;
        toggle2.isOn = false;
        toggle3.isOn = false;
        toggle4.isOn = false;
    }
}

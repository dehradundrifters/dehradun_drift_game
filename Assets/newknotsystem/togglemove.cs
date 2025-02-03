using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ToggleLocomotionAndTeleport : MonoBehaviour
{
    public Toggle modeToggle;                     // Reference to the Toggle UI element
    public LocomotionSystem locomotionSystem;     // Reference to the LocomotionSystem
    public MonoBehaviour teleporterScript;        // Reference to the Teleporter script

    void Start()
    {
        // Ensure references are set
        if (modeToggle == null || locomotionSystem == null || teleporterScript == null)
        {
            Debug.LogError("References are not properly assigned in the Inspector.");
            return;
        }

        // Subscribe to the toggle's value change event
        modeToggle.onValueChanged.AddListener(OnToggleValueChanged);

        // Set the initial state based on the toggle's current value
        UpdateState(modeToggle.isOn);
    }

    void OnDestroy()
    {
        // Unsubscribe from the toggle's value change event
        modeToggle.onValueChanged.RemoveListener(OnToggleValueChanged);
    }

    // Called when the toggle's value changes
    private void OnToggleValueChanged(bool isOn)
    {
        UpdateState(isOn);
    }

    // Updates the state of LocomotionSystem and Teleporter based on the toggle's value
    private void UpdateState(bool isOn)
    {
        if (isOn)
        {
            // Enable Teleporter and disable LocomotionSystem
            teleporterScript.enabled = true;
            locomotionSystem.enabled = false;
            Debug.Log("Teleporter enabled, LocomotionSystem disabled.");
        }
        else
        {
            // Enable LocomotionSystem and disable Teleporter
            teleporterScript.enabled = false;
            locomotionSystem.enabled = true;
            Debug.Log("LocomotionSystem enabled, Teleporter disabled.");
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ToggleReference : MonoBehaviour
{
    // Variables to hold references to the toggles
    [SerializeField] public Toggle deleteKnotToggle;
    [SerializeField] public Toggle joinKnotToggle;

    // Reference to the JoinKnot script
    private SplineLinker joinKnotScript;

    // Reference to the XRGrabInteractable component
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        // Find the XR Origin (XR Rig) in the scene
        GameObject xrOrigin = GameObject.Find("XR Origin (XR Rig)");

        if (xrOrigin != null)
        {
            // Find the Camera Offset under XR Origin
            Transform cameraOffset = FindInActiveObjectByName(xrOrigin.transform, "Camera Offset");

            if (cameraOffset != null)
            {
                // Find the Left Controller under Camera Offset
                Transform leftController = FindInActiveObjectByName(cameraOffset, "Left Controller");

                if (leftController != null)
                {
                    // Find the inactive Canvas under Left Controller
                    Transform canvas = FindInActiveObjectByName(leftController, "Canvas");

                    if (canvas != null)
                    {
                        // Find the inactive DeleteKnot toggle under Canvas
                        deleteKnotToggle = FindInActiveObjectByName(canvas, "deleteknot")?.GetComponent<Toggle>();

                        // Find the inactive JoinKnot toggle under Canvas
                        joinKnotToggle = FindInActiveObjectByName(canvas, "joinknot")?.GetComponent<Toggle>();

                        // Check if both toggles were found
                        if (deleteKnotToggle == null || joinKnotToggle == null)
                        {
                            Debug.LogError("One or both toggles (DeleteKnot, JoinKnot) were not found under Canvas.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Canvas not found under Left Controller.");
                    }
                }
                else
                {
                    Debug.LogError("Left Controller not found under Camera Offset.");
                }
            }
            else
            {
                Debug.LogError("Camera Offset not found under XR Origin.");
            }
        }
        else
        {
            Debug.LogError("XR Origin (XR Rig) not found in the scene.");
        }

        // Get the JoinKnot script attached to the same GameObject
        joinKnotScript = GetComponent<SplineLinker>();

        // Get the XRGrabInteractable component attached to the same GameObject
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Disable the JoinKnot script initially
        if (joinKnotScript != null)
        {
            joinKnotScript.enabled = false;
        }

        // Register for the grab and release events
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    // Recursive function to find inactive objects by name
    private Transform FindInActiveObjectByName(Transform parent, string name)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.name == name)
            {
                return child;
            }
            Transform result = FindInActiveObjectByName(child, name);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    // Called when the object is grabbed
    private void OnGrabbed(SelectEnterEventArgs args)
    {
        // Check if the JoinKnot toggle is on
        if (joinKnotToggle != null && joinKnotToggle.isOn)
        {
            // Enable the JoinKnot script
            if (joinKnotScript != null)
            {
                joinKnotScript.enabled = true;
                Debug.Log("JoinKnot script enabled.");
            }
        }
    }

    // Called when the object is released
    private void OnReleased(SelectExitEventArgs args)
    {
        // Disable the JoinKnot script when released
        if (joinKnotScript != null)
        {
            joinKnotScript.enabled = false;
            Debug.Log("JoinKnot script disabled.");
        }
    }

    void Update()
    {
        // Example: Log the toggle state for testing
        if (deleteKnotToggle != null && joinKnotToggle != null)
        {
            Debug.Log("Delete Knot Toggle: " + deleteKnotToggle.isOn);
            Debug.Log("Join Knot Toggle: " + joinKnotToggle.isOn);
        }
    }

    void OnDestroy()
    {
        // Unregister the event listeners
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }
}

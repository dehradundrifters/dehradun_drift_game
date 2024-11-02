using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class RightControllerActionCheck : MonoBehaviour
{
    public InputActionReference aButtonAction; // Assign in Inspector

    private void OnEnable()
    {
        aButtonAction.action.performed += OnAButtonPressed;
    }

    private void OnDisable()
    {
        aButtonAction.action.performed -= OnAButtonPressed;
    }

    private void OnAButtonPressed(InputAction.CallbackContext context)
    {
        Debug.Log("A button on the right controller is pressed.");
    }
}

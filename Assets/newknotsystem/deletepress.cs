using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance; // Singleton for global access

    [Header("Input Settings")]
    public InputActionReference rightTriggerAction; // Reference to the right trigger input action

    [Header("Toggle Settings")]
    public Toggle deleteToggle; // Toggle to enable or disable deletion functionality

    public bool IsTriggerPressed { get; private set; } // Tracks if the right trigger is currently pressed
    public bool IsDeleteEnabled => deleteToggle != null && deleteToggle.isOn; // Checks if the toggle is on

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        rightTriggerAction.action.Enable();
        rightTriggerAction.action.performed += OnTriggerPressed;
    }

    private void OnDisable()
    {
        rightTriggerAction.action.Disable();
        rightTriggerAction.action.performed -= OnTriggerPressed;
    }

    private void Update()
    {
        // Continuously check if the toggle is enabled for logging purposes
        
    }

    // Method called when the trigger is pressed
    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        IsTriggerPressed = true;

        // Log message only if the delete toggle is enabled
        if (IsDeleteEnabled)
        {
            Debug.Log("Trigger is pressed and delete toggle is ON.");
        }

        // Reset IsTriggerPressed after a short delay (to allow subsequent presses)
        Invoke(nameof(ResetTriggerState), 0.1f);
    }

    private void ResetTriggerState()
    {
        IsTriggerPressed = false;
    }
}

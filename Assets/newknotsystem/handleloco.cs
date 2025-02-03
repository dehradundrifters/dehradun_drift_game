using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.CoreUtils;

public class XRLocomotion : MonoBehaviour
{
    public XROrigin xrOrigin; // The XR Origin to move

    public InputActionProperty leftJoystick;  // Left joystick for vertical (up/down) movement
    public InputActionProperty rightJoystick; // Right joystick for forward/backward/sideways movement

    public float moveSpeed = 2.0f;     // Speed of movement (XZ)
    public float verticalSpeed = 1.5f; // Speed of vertical movement (Y)

    private void Update()
    {
        if (xrOrigin == null)
            return;

        // Read joystick inputs
        Vector2 leftInput = leftJoystick.action.ReadValue<Vector2>();  // Left joystick (up/down)
        Vector2 rightInput = rightJoystick.action.ReadValue<Vector2>(); // Right joystick (forward/backward/sideways)

        // Vertical movement (Up/Down) from left joystick
        float verticalMovement = leftInput.y * verticalSpeed * Time.deltaTime;

        // Forward/Backward & Sideways movement from right joystick
        Vector3 forwardMovement = xrOrigin.Camera.transform.forward * rightInput.y * moveSpeed * Time.deltaTime;
        Vector3 sidewaysMovement = xrOrigin.Camera.transform.right * rightInput.x * moveSpeed * Time.deltaTime;

        // Compute final movement
        Vector3 move = forwardMovement + sidewaysMovement + new Vector3(0, verticalMovement, 0);

        // Apply movement by updating the XR Origin position
        xrOrigin.transform.position += move;
    }
}

using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class AutoInteractWithKnob : MonoBehaviour
{
    private XRKnob knob;
    public XRInteractionManager interactionManager;

    private Animator leftHandAnimator;
    private Animator rightHandAnimator;

    public SphereCollider leftHandCollider;  // Assign in Inspector
    public SphereCollider rightHandCollider; // Assign in Inspector

    void Awake()
    {
        knob = GetComponent<XRKnob>();
        

        if (knob == null || interactionManager == null)
        {
            Debug.LogError("Missing XRKnob or XRInteractionManager in the scene.");
        }

        if (leftHandCollider == null || rightHandCollider == null)
        {
            Debug.LogError("Assign both SphereColliders in the inspector.");
        }
    }

    public void AutoGrab(HoverEnterEventArgs args)
    {
        if (args.interactorObject is IXRSelectInteractor interactor)
        {
            interactionManager.SelectEnter(interactor, knob);

            // Get Animator from the interacting hand
            Animator handAnimator = interactor.transform.GetComponentInChildren<Animator>();

            if (handAnimator && interactor.transform.CompareTag("LeftHand"))
            {
                leftHandAnimator = handAnimator;
                leftHandAnimator.SetFloat("Grip", 1.0f);  // Close left hand

                // Disable RightHand Collider
                if (rightHandCollider != null)
                    rightHandCollider.enabled = false;
            }
            else if (handAnimator && interactor.transform.CompareTag("RightHand"))
            {
                rightHandAnimator = handAnimator;
                rightHandAnimator.SetFloat("Grip", 1.0f);  // Close right hand

                // Disable LeftHand Collider
                if (leftHandCollider != null)
                    leftHandCollider.enabled = false;
            }
        }
    }

    public void AutoRelease(HoverExitEventArgs args)
    {
        if (args.interactorObject is IXRSelectInteractor interactor)
        {
            interactionManager.SelectExit(interactor, knob);

            if (interactor.transform.CompareTag("LeftHand") && leftHandAnimator)
            {
                leftHandAnimator.SetFloat("Grip", 0.0f);  // Open left hand
                leftHandAnimator = null;

                // Enable RightHand Collider
                if (rightHandCollider != null)
                    rightHandCollider.enabled = true;
            }
            else if (interactor.transform.CompareTag("RightHand") && rightHandAnimator)
            {
                rightHandAnimator.SetFloat("Grip", 0.0f);  // Open right hand
                rightHandAnimator = null;

                // Enable LeftHand Collider
                if (leftHandCollider != null)
                    leftHandCollider.enabled = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampForceApplier : MonoBehaviour
{
    public float additionalForce = 2000f; // Adjustable force amount
     // Tag for the ramp trigger collider

    private Rigidbody carRigidbody;

    void Start()
    {
        // Get the Rigidbody component attached to the car
        carRigidbody = GetComponent<Rigidbody>();

        if (carRigidbody == null)
        {
            Debug.LogError("No Rigidbody found on the car. Please add a Rigidbody component.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the trigger collider has the specified tag
        if (other.CompareTag("ramp2x") && carRigidbody != null)
        {
            additionalForce = 2000f;
            // Apply additional force in the car's forward direction
            Vector3 forceDirection = transform.forward * additionalForce;
            carRigidbody.AddForce(forceDirection, ForceMode.Impulse);

            Debug.Log("Additional force applied: " + additionalForce);
        }
        else if (other.CompareTag("ramp4x") && carRigidbody != null)
        {
            additionalForce = 4000f;
            // Apply additional force in the car's forward direction
            Vector3 forceDirection = transform.forward * additionalForce;
            carRigidbody.AddForce(forceDirection, ForceMode.Impulse);

            Debug.Log("Additional force applied: " + additionalForce);
        }
        else if (other.CompareTag("ramp6x") && carRigidbody != null)
        {
            additionalForce = 6000f;
            // Apply additional force in the car's forward direction
            Vector3 forceDirection = transform.forward * additionalForce;
            carRigidbody.AddForce(forceDirection, ForceMode.Impulse);

            Debug.Log("Additional force applied: " + additionalForce);
        }
    }
}

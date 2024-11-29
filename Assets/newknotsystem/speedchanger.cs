using UnityEngine;

public class ChangeTagOnTrigger : MonoBehaviour
{
    public string changetriggerto;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the specified tag
        if (other.CompareTag("ramp2x") || other.CompareTag("ramp4x")|| other.CompareTag("ramp6x"))
        {
            // Change the tag of the object
            other.tag = changetriggerto;
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class cubeoff : MonoBehaviour
{
    public GameObject cube;
    public InputActionProperty rightTriggerAction;
    public InputActionProperty rightAction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rightTriggerAction.action.WasPressedThisFrame())
        {
            cube.SetActive(false);
        }
        if (rightAction.action.WasPressedThisFrame())
        {
            Debug.Log("A button was pressed ");
        }

    }
}

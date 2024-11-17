using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class openmenu : MonoBehaviour
{
    public InputActionProperty leftTriggerAction;
    public GameObject Handmenu;

    // Track if the menu is currently active
    private bool isMenuOpen = false;

    // Update is called once per frame
    void Update()
    {
        // Check if the button was pressed this frame
        if (leftTriggerAction.action.WasPressedThisFrame())
        {
            //Debug.Log("open the menu");
            // Toggle the state of the hand menu
            isMenuOpen = !isMenuOpen;

            // Set the menu's active state based on the toggle
            Handmenu.SetActive(isMenuOpen);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public void OnMovementInput(InputAction.CallbackContext context)
    {
        Debug.Log("Moving");
    }

    //Context -> value, phase(when it was started, performed and cancelled), 

    public void OnDashInput(InputAction.CallbackContext context)
    {

    }
}

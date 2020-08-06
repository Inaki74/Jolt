using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public Vector2 MovementVector { get; private set; }

    public bool DashBegin { get; private set; }

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        MovementVector = context.ReadValue<Vector2>();
    }

    //Context -> value, phase(when it was started, performed and cancelled), 

    public void OnBeginDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashBegin = true;
        }

        if (context.canceled)
        {
            DashBegin = false;
        }
    }
}

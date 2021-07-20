using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopInputController : IInputController
{
    public void ManageDash(ref bool dashBegin, ref Vector3 initialDashPoint, ref Vector3 finalDashPoint)
    {
        if (Input.GetMouseButtonDown(0))
        {
            dashBegin = true;
            initialDashPoint = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            dashBegin = false;
            finalDashPoint = Input.mousePosition;
        }
    }

    public void ManageJump(bool jumpingBool)
    {
        throw new System.NotImplementedException();
    }

    public void ManageMovement(ref Vector2 movementVector)
    {
        float horizontalInput = Input.GetAxis("Horizontal");


        movementVector.Set(horizontalInput, movementVector.y);
    }

    private void AssureFixedMovementSpeed(ref float horizontalInput)
    {
        if (horizontalInput > 0)
        {
            horizontalInput = 1;
        }

        if (horizontalInput < 0)
        {
            horizontalInput = -1;
        }
    }
}

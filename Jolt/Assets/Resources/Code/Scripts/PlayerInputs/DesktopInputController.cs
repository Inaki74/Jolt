using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopInputController : IInputController
{
    public void ManageDash(ref bool dashBegin, ref Vector3 initialDashPoint, ref Vector3 finalDashPoint)
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);

        Debug.Log(inputVector);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dashBegin = true;
            initialDashPoint = Vector3.zero;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            dashBegin = false;
            initialDashPoint = inputVector;
        }

        //MouseDash(ref dashBegin, ref initialDashPoint, ref finalDashPoint);
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

    private void MouseDash(ref bool dashBegin, ref Vector3 initialDashPoint, ref Vector3 finalDashPoint)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopInputController : IInputController
{
    public void ManageDash(ref bool dashBegin, ref Vector3 initialDashPoint, ref Vector3 finalDashPoint)
    {
        KeyboardDash(ref dashBegin, ref initialDashPoint, ref finalDashPoint);

        //MouseDash(ref dashBegin, ref initialDashPoint, ref finalDashPoint);
    }

    public void ManageJump(bool jumpingBool)
    {
        throw new System.NotImplementedException();
    }

    public void ManageMovement(ref Vector2 movementVector)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        movementVector.Set(horizontalInput, movementVector.y);
    }

    private void KeyboardDash(ref bool dashBegin, ref Vector3 initialDashPoint, ref Vector3 finalDashPoint)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dashBegin = true;
            initialDashPoint = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            finalDashPoint = inputVector;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            dashBegin = false;
            
        }
    }

    private void MouseDash(ref bool dashBegin, ref Vector3 initialDashPoint, ref Vector3 finalDashPoint)
    {
        if (Input.GetMouseButtonDown(0))
        {
            dashBegin = true;
            initialDashPoint = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            finalDashPoint = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            dashBegin = false;
            
        }
    }

    private void AssureFixedMovementSpeed(ref float input)
    {
        if (input > 0)
        {
            input = 1;
        }

        if (input < 0)
        {
            input = -1;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerInput
        {
            public class JoystickInputController : IInputController
            {
                public void ManageDash(ref bool dashBegin, ref Vector3 initialDashPoint, ref Vector3 finalDashPoint)
                {
                    float horizontalInput = Input.GetAxis("HorizontalJoy");
                    float verticalInput = Input.GetAxis("VerticalJoy");

                    Debug.Log("Horizontal: " + horizontalInput);
                    Debug.Log("Vertical: " + verticalInput);

                    Vector2 inputVector = new Vector2(horizontalInput, verticalInput);

                    if (Input.GetButtonDown("DashJoy"))
                    {
                        dashBegin = true;
                        initialDashPoint = Vector3.zero;
                    }

                    if (Input.GetButton("DashJoy"))
                    {
                        finalDashPoint = inputVector;
                    }

                    if (Input.GetButtonUp("DashJoy"))
                    {
                        dashBegin = false;
                    }
                }

                public void ManageJump(bool jumpingBool)
                {
                    throw new System.NotImplementedException();
                }

                public void ManageMovement(ref Vector2 movementVector)
                {
                    float horizontalInput = Input.GetAxisRaw("HorizontalJoy");

                    movementVector.Set(horizontalInput, movementVector.y);
                }
            }
        }
    }
}



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
                    float horizontalInput = Input.GetAxisRaw("Horizontal");
                    float verticalInput = Input.GetAxisRaw("Vertical");

                    Vector2 inputVector = new Vector2(horizontalInput, verticalInput);

                    if (Input.GetButtonDown("joystick button 2"))
                    {
                        dashBegin = true;
                        initialDashPoint = Vector3.zero;
                    }

                    if (Input.GetButton("joystick button 2"))
                    {
                        finalDashPoint = inputVector;
                    }

                    if (Input.GetButtonUp("joystick button 2"))
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
                    float horizontalInput = Input.GetAxis("Horizontal");

                    movementVector.Set(horizontalInput, movementVector.y);
                }
            }
        }
    }
}



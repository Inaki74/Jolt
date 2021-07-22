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
                    float horizontalInput = Input.GetAxis(InputStringNames.JOYSTICK_HORIZONTAL_NAME);
                    float verticalInput = Input.GetAxis(InputStringNames.JOYSTICK_VERTICAL_NAME);

                    Vector2 inputVector = new Vector2(horizontalInput, verticalInput);

                    if (Input.GetButtonDown(InputStringNames.JOYSTICK_DASH_NAME))
                    {
                        dashBegin = true;
                        initialDashPoint = Vector3.zero;
                    }

                    if (Input.GetButton(InputStringNames.JOYSTICK_DASH_NAME))
                    {
                        finalDashPoint = inputVector;
                    }

                    if (Input.GetButtonUp(InputStringNames.JOYSTICK_DASH_NAME))
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
                    float horizontalInput = Input.GetAxisRaw(InputStringNames.JOYSTICK_HORIZONTAL_NAME);

                    movementVector.Set(horizontalInput, movementVector.y);
                }
            }
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerInput
        {
            public class DesktopInputController : IInputController
            {
                private bool _mouse;

                public DesktopInputController(bool mouse)
                {
                    _mouse = mouse;
                }

                public void ManageDash(ref bool dashBegin, ref Vector3 initialDashPoint, ref Vector3 finalDashPoint)
                {
                    if (_mouse)
                    {
                        MouseDash(ref dashBegin, ref initialDashPoint, ref finalDashPoint);
                    }
                    else
                    {
                        KeyboardDash(ref dashBegin, ref initialDashPoint, ref finalDashPoint);
                    }
                }

                public void ManageJump(bool jumpingBool)
                {
                    throw new System.NotImplementedException();
                }

                public void ManageMovement(ref Vector2 movementVector)
                {
                    float horizontalInput = Input.GetAxisRaw(InputStringNames.KEYBOARD_HORIZONTAL_NAME);

                    movementVector.Set(horizontalInput, movementVector.y);
                }

                private void KeyboardDash(ref bool dashBegin, ref Vector3 initialDashPoint, ref Vector3 finalDashPoint)
                {
                    float horizontalInput = Input.GetAxisRaw(InputStringNames.KEYBOARD_HORIZONTAL_NAME);
                    float verticalInput = Input.GetAxisRaw(InputStringNames.KEYBOARD_VERTICAL_NAME);

                    Vector2 inputVector = new Vector2(horizontalInput, verticalInput);

                    if (Input.GetButtonDown(InputStringNames.KEYBOARD_DASH_NAME))
                    {
                        dashBegin = true;
                        initialDashPoint = Vector3.zero;
                    }

                    if (Input.GetButton(InputStringNames.KEYBOARD_DASH_NAME))
                    {
                        finalDashPoint = inputVector;
                    }

                    if (Input.GetButtonUp(InputStringNames.KEYBOARD_DASH_NAME))
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
        }
    }
}



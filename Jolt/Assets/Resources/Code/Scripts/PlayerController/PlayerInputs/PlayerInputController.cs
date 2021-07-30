using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerInput
        {
            public class PlayerInputController
            {
                private IInputController _inputController;

                public void ManageDash(ref bool dashBegin, ref Vector3 initialDashPoint, ref Vector3 finalDashPoint)
                {
                    float horizontalInput = _inputController.Horizontal;
                    float verticalInput = _inputController.Vertical;

                    Vector2 inputVector = new Vector2(horizontalInput, verticalInput);

                    

                    if (_inputController.DashHold)
                    {
                        dashBegin = false;
                        //if (_inputController.Pointer)
                        //{
                        //    finalDashPoint = _inputController.PointerVector;
                        //}
                        //else
                        //{
                        //    finalDashPoint = inputVector;
                        //}
                    }

                    if (_inputController.DashUp)
                    {
                        dashBegin = false;
                    }

                    if (_inputController.DashDown)
                    {
                        dashBegin = true;
                        finalDashPoint = inputVector;
                    }
                }

                public void ManageJump(ref bool jumpingBool)
                {
                    bool jumpInput = _inputController.JumpDown || _inputController.JumpHold;

                    jumpingBool = jumpInput;
                }

                public void ManageMovement(ref Vector2 movementVector)
                {
                    float horizontalInput = _inputController.Horizontal;

                    movementVector.Set(horizontalInput, movementVector.y);
                }

                public void SetInputController(IInputController inputController)
                {
                    _inputController = inputController;
                }
            }
        }
    }
}



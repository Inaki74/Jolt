using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jolt
{
    namespace PlayerController
    {
        using PlayerInput;

        public class PlayerInputManager : MonoBehaviour
        {
            private IInputController _inputController;

            private Vector2 _movementVector;
            public Vector2 MovementVector
            {
                get
                {
                    return _movementVector;
                }
                set
                {
                    _movementVector = value;
                }
            }

            private bool _dashBegin;
            public bool DashBegin
            {
                get
                {
                    return _dashBegin;
                }
                set
                {
                    _dashBegin = value;
                }
            }

            private Vector3 _initialDashPoint;
            public Vector3 InitialDashPoint
            {
                get
                {
                    return _initialDashPoint;
                }
                set
                {
                    _initialDashPoint = value;
                }
            }

            private Vector3 _finalDashPoint;
            public Vector3 FinalDashPoint
            {
                get
                {
                    return _finalDashPoint;
                }
                set
                {
                    _finalDashPoint = value;
                }
            }

            private float dashCD = 0.2f;
            private float currentDashCD;

            private void Start()
            {
                if (SystemInfo.deviceType == DeviceType.Console)
                {
                    _inputController = new JoystickInputController();
                }

                if (SystemInfo.deviceType == DeviceType.Desktop)
                {
                    _inputController = new DesktopInputController();
                }

                if (SystemInfo.deviceType == DeviceType.Handheld)
                {
                    _inputController = new MobileInputController();
                }

                //currentDashCD = -1;
                //runOnce = false;
                //found = false;
            }

            private void Update()
            {
                _inputController.ManageMovement(ref _movementVector);

                _inputController.ManageDash(ref _dashBegin, ref _initialDashPoint, ref _finalDashPoint);
            }



            /// <summary>
            /// NEW SYSTEM
            /// </summary>
            //Context -> value, phase(when it was started, performed and cancelled), 
            private bool runOnce;
            private bool found;
            private bool moving;
            private int foundTouchId;

            public void OnBeginDashInput(InputAction.CallbackContext context)
            {
                Debug.Log("pene");

                if (moving)
                {
                    StartCoroutine("CoAux");
                }
                else
                {
                    if (!runOnce)
                    {
                        StartCoroutine("CoManageTouches");
                    }
                }

            }

            private IEnumerator CoAux()
            {
                while (moving)
                {
                    if (!runOnce)
                    {
                        StartCoroutine("CoManageTouches");
                    }
                    yield return new WaitForEndOfFrame();
                }

            }

            private IEnumerator CoManageTouches()
            {
                for (int i = 0; i < Touchscreen.current.touches.Count && !found; i++)
                {
                    //Find the first touch that is not using the joystick and is not null
                    if (ValidateInitialDashPoint(Touchscreen.current.touches[i].startPosition.ReadValue(), 0, 400, 400, 0) &&
                        Touchscreen.current.touches[i].startPosition.ReadValue() != Vector2.zero)
                    {
                        runOnce = true;
                        found = true;

                        if (Touchscreen.current.touches[i].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began && currentDashCD < 0)
                        {
                            DashBegin = true;
                            InitialDashPoint = Touchscreen.current.touches[i].startPosition.ReadValue();
                        }

                        while (DashBegin)
                        {
                            FinalDashPoint = Touchscreen.current.touches[i].position.ReadValue();

                            if (Touchscreen.current.touches[i].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
                            {
                                DashBegin = false;
                                currentDashCD = dashCD;
                            }
                            yield return new WaitForEndOfFrame();
                        }
                    }
                }

                found = false;
                runOnce = false;
            }

            //Returns false if its inside the selected bounds (not valid)
            private bool ValidateInitialDashPoint(Vector2 point, float xLowerBound, float yHigherBound, float xHigherBound, float yLowerBound)
            {
                return !(point.x > xLowerBound && point.x < xHigherBound && point.y > yLowerBound && point.y < yHigherBound);
            }

            //TRY 1:
            //This is a working touch, just in case ill leave it here
            //bool isValid = ValidateInitialDashPoint(Touchscreen.current.primaryTouch.position.ReadValue(), 50, 290, 300, 40);

            //            if ((Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began && isValid) && currentDashCD< 0)
            //            {
            //                DashBegin = true;
            //                InitialDashPoint = Touchscreen.current.primaryTouch.position.ReadValue();
            //            }

            //            bool wasValid = ValidateInitialDashPoint(InitialDashPoint, 50, 290, 300, 40);

            //            if ((Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended && wasValid))
            //            {
            //                DashBegin = false;
            //                currentDashCD = dashCD;
            //            }

            //TRY 2:
            //if (Touchscreen.current.touches[foundTouch].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began && currentDashCD< 0)
            //        {
            //            DashBegin = true;
            //            InitialDashPoint = Touchscreen.current.touches[foundTouch].startPosition.ReadValue();
            //        }

            //        if (Touchscreen.current.touches[foundTouch].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
            //        {
            //            DashBegin = false;
            //            currentDashCD = dashCD;
            //        }

            //TRY 3(best competitor):
            //    Debug.Log("Run1");
            //        if (!runOnce)
            //        {
            //            Debug.Log("Run2");
            //            StartCoroutine("CoManageTouches");
            //}



            //TRY 4:
            //foundTouchId = DecideTouch();

            //UnityEngine.InputSystem.Controls.TouchControl foundTouch = FindTouch(Touchscreen.current.touches.ToArray(), foundTouchId);
            ////runOnce = true;

            //Debug.Log(foundTouch);

            //        if(foundTouch != null)
            //        {
            //            Debug.Log(foundTouch.phase.ReadValue());
            //            if (foundTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began && currentDashCD< 0)
            //            {
            //                DashBegin = true;
            //                InitialDashPoint = foundTouch.startPosition.ReadValue();
            //            }

            //            if(foundTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved)
            //            {
            //                FinalDashPoint = foundTouch.position.ReadValue();
            //            }

            //            if (foundTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
            //            {
            //                DashBegin = false;
            //                //runOnce = false;
            //                currentDashCD = dashCD;
            //            }
            //        }
            //        else
            //        {
            //            runOnce = false;
            //            DashBegin = false;
            //        }

            private int DecideTouch()
            {
                for (int i = 0; i < Touchscreen.current.touches.Count; i++)
                {
                    //Find all the touches that are not using the joystick, use the first one
                    if (ValidateInitialDashPoint(Touchscreen.current.touches[i].startPosition.ReadValue(), 50, 290, 300, 40) &&
                        Touchscreen.current.touches[i].startPosition.ReadValue() != Vector2.zero)
                        return Touchscreen.current.touches[i].touchId.ReadValue();
                }

                return -1;
            }

            private UnityEngine.InputSystem.Controls.TouchControl FindTouch(UnityEngine.InputSystem.Controls.TouchControl[] touches, int id)
            {
                for (int i = 0; i < touches.Length; i++)
                {
                    //Find all the touches that are not using the joystick, use the first one
                    if (touches[i].touchId.ReadValue() == id)
                        return touches[i];
                }

                return null;
            }
        }
    }
}


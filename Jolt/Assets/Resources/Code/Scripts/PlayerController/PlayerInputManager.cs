using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        using PlayerInput;

        public enum ControlSchemeAux
        {
            CONTROLLER,
            KEYBOARD,
            KEYBOARDANDMOUSE
        }

        public class PlayerInputManager : MonoBehaviour, IPlayerInputManager
        {
            private const int DASH_BUFFER_AMOUNT = 2;
            private const float JUMP_TIMEOUT_TIME = 0.1f;

            [SerializeField]
            private ControlSchemeAux _controlScheme;

            private PlayerInputController _playerInputController;

            private Vector2 _movementVector;
            public Vector2 MovementVector { get => _movementVector; set => _movementVector = value; }

            
            private bool _dashBegin;
            private Queue<bool> _dashQueue = new Queue<bool>(DASH_BUFFER_AMOUNT);
            public bool DashBegin
            {
                get
                {
                    if(_dashQueue.Count != 0)
                    {
                        return _dashQueue.Dequeue();
                    }

                    return false;
                }
            }

            private Vector3 _initialDashPoint;
            public Vector3 InitialDashPoint { get => _initialDashPoint; set => _initialDashPoint = value; }

            private Vector3 _finalDashPoint;
            public Vector3 FinalDashPoint { get => _finalDashPoint; set => _finalDashPoint = value; }

            private float _jumpPressedTimeout = 0f;
            private bool _jumpPressed;
            //public bool JumpPressed { get => _jumpPressed; set => _jumpPressed = value; }
            public bool JumpPressed { get => _jumpPressedTimeout < JUMP_TIMEOUT_TIME; set => _jumpPressed = value; }

            private bool _jumpHeld;
            public bool JumpHeld { get => _jumpHeld; set => _jumpHeld = value; }
            public bool Disabled { get; set; }

            private void Start()
            {
                DecideControlScheme();
                _jumpPressedTimeout = JUMP_TIMEOUT_TIME;
            }

            private void DecideControlScheme()
            {
                _playerInputController = new PlayerInputController();
                IInputController inputController = null;

                if (SystemInfo.deviceType == DeviceType.Console)
                {
                    inputController = new JoystickInputController();
                }

                if (SystemInfo.deviceType == DeviceType.Desktop)
                {
                    inputController = new KeyboardInputController();
                }

                if (SystemInfo.deviceType == DeviceType.Handheld)
                {
                    inputController = new MobileInputController();
                }

                // For testing purposes.
                switch (_controlScheme)
                {
                    case ControlSchemeAux.KEYBOARD:
                        inputController = new KeyboardInputController();
                        break;
                    case ControlSchemeAux.KEYBOARDANDMOUSE:
                        inputController = new KeyboardAndMouseInputController();
                        break;
                    case ControlSchemeAux.CONTROLLER:
                        inputController = new JoystickInputController();
                        break;
                }

                _playerInputController.SetInputController(inputController);
            }

            private void Update()
            {
                if (Disabled)
                {
                    return;
                }

                _playerInputController.ManageMovement(ref _movementVector);

                _playerInputController.ManageDash(ref _dashBegin, ref _finalDashPoint);

                _playerInputController.ManageJump(ref _jumpPressed, ref _jumpHeld);

                if (_dashBegin && _dashQueue.Count < DASH_BUFFER_AMOUNT)
                {
                    _dashQueue.Enqueue(_dashBegin);
                }

                ManageJumpInput();
            }

            private void ManageJumpInput()
            {
                if (_jumpPressed)
                {
                    _jumpPressedTimeout = 0f;
                }

                if (_jumpPressedTimeout < JUMP_TIMEOUT_TIME)
                {
                    _jumpPressedTimeout += Time.deltaTime;
                }
            }

            public void ResetJumpTimer()
            {
                _jumpPressedTimeout = JUMP_TIMEOUT_TIME;
            }
        }
    }
}


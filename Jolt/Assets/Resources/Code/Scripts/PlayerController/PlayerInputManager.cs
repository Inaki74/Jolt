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
            [SerializeField]
            private ControlSchemeAux _controlScheme;

            private PlayerInputController _playerInputController;

            private Vector2 _movementVector;
            public Vector2 MovementVector { get => _movementVector; set => _movementVector = value; }

            private const int _dashBufferAmount = 2;
            private bool _dashBegin;
            private Queue<bool> _dashQueue = new Queue<bool>(_dashBufferAmount);
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

            private bool _jumpPressed;
            public bool JumpPressed { get => _jumpPressed; set => _jumpPressed = value; }

            private bool _jumpHeld;
            public bool JumpHeld { get => _jumpHeld; set => _jumpHeld = value; }

            private void Start()
            {
                DecideControlScheme();
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
                _playerInputController.ManageMovement(ref _movementVector);

                _playerInputController.ManageDash(ref _dashBegin, ref _finalDashPoint);

                if (_dashBegin && _dashQueue.Count < _dashBufferAmount)
                {
                    _dashQueue.Enqueue(_dashBegin);
                }
                

                _playerInputController.ManageJump(ref _jumpPressed, ref _jumpHeld);
            }
        }
    }
}


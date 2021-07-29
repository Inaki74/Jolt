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
                public float Horizontal => Input.GetAxisRaw(InputStringNames.JOYSTICK_HORIZONTAL_NAME);

                public float Vertical => Input.GetAxisRaw(InputStringNames.JOYSTICK_VERTICAL_NAME);

                public bool DashDown => Input.GetButtonDown(InputStringNames.JOYSTICK_DASH_NAME);

                public bool DashHold => Input.GetButton(InputStringNames.JOYSTICK_DASH_NAME);

                public bool DashUp => Input.GetButtonUp(InputStringNames.JOYSTICK_DASH_NAME);

                public bool Pointer => false;

                public Vector2 PointerVector => Vector2.zero;
            }
        }
    }
}



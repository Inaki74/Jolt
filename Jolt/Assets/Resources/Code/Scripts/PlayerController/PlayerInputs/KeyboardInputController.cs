using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerInput
        {
            public class KeyboardInputController : IInputController
            {
                public float Horizontal => Input.GetAxisRaw(InputStringNames.KEYBOARD_HORIZONTAL_NAME);

                public float Vertical => Input.GetAxisRaw(InputStringNames.KEYBOARD_VERTICAL_NAME);

                public bool DashDown => Input.GetButtonDown(InputStringNames.KEYBOARD_DASH_NAME);

                public bool DashHold => Input.GetButton(InputStringNames.KEYBOARD_DASH_NAME);

                public bool DashUp => Input.GetButtonUp(InputStringNames.KEYBOARD_DASH_NAME);

                public bool Pointer => false;

                public Vector2 PointerVector => Vector2.zero;
            }
        }
    }
}



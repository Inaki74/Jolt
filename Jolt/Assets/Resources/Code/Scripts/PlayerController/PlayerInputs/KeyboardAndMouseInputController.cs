using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerInput
        {
            public class KeyboardAndMouseInputController : IInputController
            {
                public float Horizontal => Input.GetAxisRaw(InputStringNames.KEYBOARD_HORIZONTAL_NAME);

                public float Vertical => Input.GetAxisRaw(InputStringNames.KEYBOARD_VERTICAL_NAME);

                public bool DashDown => Input.GetMouseButtonDown(0);

                public bool DashHold => Input.GetMouseButton(0);

                public bool DashUp => Input.GetMouseButtonUp(0);

                public bool Pointer => true;

                public Vector2 PointerVector => Input.mousePosition;
            }
        }
    }
}



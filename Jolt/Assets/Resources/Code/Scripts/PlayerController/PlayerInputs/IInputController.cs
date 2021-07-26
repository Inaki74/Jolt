using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerInput
        {
            public interface IInputController
            {
                float Horizontal { get; }
                float Vertical { get; }
                bool DashDown { get; }
                bool DashHold { get; }
                bool DashUp { get; }
                bool Pointer { get; }
                Vector2 PointerVector { get; }
            }
        }
    }
}



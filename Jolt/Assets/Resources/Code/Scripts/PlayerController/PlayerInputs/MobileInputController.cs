using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerInput
        {
            public class MobileInputController : IInputController
            {
                public float Horizontal => throw new System.NotImplementedException();

                public float Vertical => throw new System.NotImplementedException();

                public bool DashDown => throw new System.NotImplementedException();

                public bool DashHold => throw new System.NotImplementedException();

                public bool DashUp => throw new System.NotImplementedException();

                public bool Pointer => false;

                public Vector2 PointerVector => Vector2.zero;
            }
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        public interface IPlayerController
        {
            void Move(Vector2 direction, float speed);
            void MoveX(float direction, float speed);
            void MoveY(float direction, float speed);

        }
    }
}



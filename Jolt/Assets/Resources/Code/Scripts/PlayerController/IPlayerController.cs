using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        public interface IPlayerController
        {
            void Move(Vector2 direction);
            void MoveTowards(Vector2 destination, float speed);
        }
    }
}



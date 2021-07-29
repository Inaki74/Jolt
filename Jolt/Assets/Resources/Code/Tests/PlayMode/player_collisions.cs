using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Jolt
{
    namespace Tests
    {
        public class player_collisions
        {
            // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
            // `yield return null;` to skip a frame.
            [UnityTest]
            public IEnumerator m()
            {
                // Use the Assert class to test conditions.
                // Use yield to skip a frame.
                yield return null;
            }
        }
    }
}



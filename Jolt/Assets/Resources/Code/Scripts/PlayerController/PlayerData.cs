using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        [CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
        public class PlayerData : ScriptableObject
        {
            [Header("Move State Variables")]
            public float movementSpeed = 6.0f;

            [Header("Pre-Dash State Variables")]
            public float timeSlow = 0.1f;
            public float preDashTimeOut;
            public int amountOfDashes;
            public float circleRadius;

            [Header("Dashing State Variables")]
            public float dashTimeOut;
            public float dashSpeed;

            [Header("Checks Variables")]
            public float checkGroundRadius = 0.05f;
            public LayerMask whatIsGround;

            [Header("Rail States")]
            public List<Vector2> allPaths = new List<Vector2>();

            [Header("Dead State")]
            public float deadTimer = 5f;
            public Vector2 lastCheckpoint;

            public float Test { get; set; }
        }
    }
}



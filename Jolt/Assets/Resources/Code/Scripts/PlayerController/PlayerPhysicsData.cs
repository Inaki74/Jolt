﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        [CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Physics Data")]
        public class PlayerPhysicsData : ScriptableObject, IPlayerPhysicsData
        {
            [SerializeField] private Vector2 _standardScale;

            [SerializeField] private float _standardGravity;

            [SerializeField] private float _standardMaxFallSpeed;

            public Vector2 StandardScale { get { return _standardScale; } }
            public float StandardGravity { get { return _standardGravity; } }
            public float StandardMaxFallSpeed { get { return _standardMaxFallSpeed; } }
        }
    }
}

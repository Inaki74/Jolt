using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        [CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
        public class PlayerData : ScriptableObject, IPlayerData
        {
            [Header("Move State Variables")]
            [SerializeField] private float _movementSpeed = 6.0f;

            [Header("Pre-Dash State Variables")]
            [SerializeField] private float _timeSlow = 0.1f;
            [SerializeField] private float _preDashTimeOut;
            [SerializeField] private int _amountOfDashes;
            [SerializeField] private float _circleRadius;

            [Header("Dashing State Variables")]
            [SerializeField] private float _dashTimeOut;
            [SerializeField] private float _dashSpeed;

            [Header("Checks Variables")]
            [SerializeField] private float _checkGroundRadius = 0.05f;
            [SerializeField] private LayerMask _whatIsGround;

            [Header("Rail States")]
            [SerializeField] private List<Vector2> _allPaths = new List<Vector2>();

            [Header("Dead State")]
            [SerializeField] private float _deadTimer = 5f;

            [SerializeField] private Vector2 _lastCheckpoint;

            [Header("Recoil State")]
            [SerializeField] private float _recoilTimer = 0.2f;

            [Header("Jumping State")]
            [SerializeField] private float _jumpForce;

            public float MovementSpeed { get { return _movementSpeed; } }

            public float TimeSlow { get { return _timeSlow; } }

            public float PreDashTimeOut { get { return _preDashTimeOut; } }

            public int AmountOfDashes { get { return _amountOfDashes; } }

            public float CircleRadius { get { return _circleRadius; } }

            public float DashTimeOut { get { return _dashTimeOut; } }

            public float DashSpeed { get { return _dashSpeed; } }

            public float CheckGroundRadius { get { return _checkGroundRadius; } }

            public LayerMask WhatIsGround { get { return _whatIsGround; } }

            public List<Vector2> AllPaths { get { return _allPaths; } }

            public float DeadTimer { get { return _deadTimer; } }

            public Vector2 LastCheckpoint { get { return _lastCheckpoint; } }

            public float RecoilTimer { get { return _recoilTimer; } }

            public float JumpForce { get { return _jumpForce; } }
        }
    }
}



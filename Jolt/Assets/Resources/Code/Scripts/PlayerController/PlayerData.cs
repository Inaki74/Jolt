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
            [Header("Rigidbody Variables")]
            [SerializeField] private PlayerPhysicsData _playerPhysicsData;

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
            [SerializeField] private float _checkWallRadius;
            [SerializeField] private float _checkGroundRadius = 0.05f;
            [SerializeField] private LayerMask _whatIsGround;

            [Header("Rail States")]
            [SerializeField] private List<Vector2> _allPaths = new List<Vector2>();

            [Header("Dead State")]
            [SerializeField] private float _deadTimer = 5f;

            [SerializeField] private Vector2 _lastCheckpoint;

            [Header("Recoil State")]
            [SerializeField] private float _recoilTimer = 0.2f;

            [Header("Airborne State")]
            [SerializeField] private float _freeFallGravity;

            [Header("Jumping State")]
            [SerializeField] private float _jumpForce;
            [SerializeField] private float _jumpGravity;
            [SerializeField] private float _jumpDrag;
            [SerializeField] private float _coyoteTiming;

            [Header("Wall Slide State")]
            [SerializeField] private float _wallSlideGravity;
            [SerializeField] private float _wallSlideDrag;
            [SerializeField] private float _inverseMultiplierOfFallSpeed;

            [Header("Wall Jump State")]
            [SerializeField] private float _wallJumpForceHorizontal;
            [SerializeField] private float _wallJumpForceVerticalRatioWithHorizontal;

            public IPlayerPhysicsData PlayerPhysicsData { get { return _playerPhysicsData;  } }

            public float MovementSpeed { get { return _movementSpeed; } }

            public float TimeSlow { get { return _timeSlow; } }

            public float PreDashTimeOut { get { return _preDashTimeOut; } }

            public int AmountOfDashes { get { return _amountOfDashes; } }

            public float CircleRadius { get { return _circleRadius; } }

            public float DashTimeOut { get { return _dashTimeOut; } }

            public float DashSpeed { get { return _dashSpeed; } }

            public float CheckGroundRadius { get { return _checkGroundRadius; } }

            public float CheckWallRadius { get { return _checkWallRadius; } }

            public LayerMask WhatIsGround { get { return _whatIsGround; } }

            public List<Vector2> AllPaths { get { return _allPaths; } }

            public float DeadTimer { get { return _deadTimer; } }

            public Vector2 LastCheckpoint { get { return _lastCheckpoint; } }

            public float RecoilTimer { get { return _recoilTimer; } }

            public float FreeFallGravity { get { return _freeFallGravity;  } }

            public float JumpForce { get { return _jumpForce; } }

            public float JumpGravity { get { return _jumpGravity; } }

            public float JumpDrag { get { return _jumpDrag; } }

            public float CoyoteTiming { get { return _coyoteTiming; } }

            public float WallSlideGravity { get { return _wallSlideGravity; } }

            public float WallSlideDrag { get { return _wallSlideDrag; } }

            public float InverseMultiplierOfFallSpeed { get { return _inverseMultiplierOfFallSpeed; } }

            public float WallJumpForceHorizontal { get { return _wallJumpForceHorizontal; } }

            public float WallJumpForceVerticalRatioWithHorizontal { get { return _wallJumpForceVerticalRatioWithHorizontal;  } }
        }
    }
}



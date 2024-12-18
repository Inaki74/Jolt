﻿using System.Collections;
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

            [Header("Dash Floating State Variables")]
            [SerializeField] private float _dashFloatingTimeout;
            [SerializeField] private float _dashFloatingGravityScale;

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
            [SerializeField] private float _freeFallMaxFallSpeed;
            [SerializeField] private float _maxDeformedScale;
            [SerializeField] private float _scaleReductionModifier;

            [Header("Jumping State")]
            [SerializeField] private float _jumpForce;
            [SerializeField] private float _jumpGravity;
            [SerializeField] private float _jumpCoyoteTiming;

            [Header("Floating State")]
            [SerializeField] private float _floatGravity;
            [SerializeField] private float _floatingGravityScaleIntoWall;

            [Header("Wall Slide State")]
            [SerializeField] private float _startingWallSlideGravity;
            [SerializeField] private float _finalWallSlideGravity;
            [SerializeField] private float _timeToReachFinalGravity;
            [SerializeField] private float _startingFallSpeed;
            [SerializeField] private float _wallSlideMaxFallSpeed;

            [Header("Wall Jump State")]
            [SerializeField] private float _wallJumpDuration;
            [SerializeField] private float _wallJumpGravity;
            [SerializeField] private float _wallJumpForceHorizontal;
            [SerializeField] private float _wallJumpForceVertical;
            [SerializeField] private float _wallJumpCoyoteTiming;

            public IPlayerPhysicsData PlayerPhysicsData { get { return _playerPhysicsData;  } }

            public float MovementSpeed { get { return _movementSpeed; } }

            public float TimeSlow { get { return _timeSlow; } }

            public float PreDashTimeOut { get { return _preDashTimeOut; } }

            public int AmountOfDashes { get { return _amountOfDashes; } }

            public float CircleRadius { get { return _circleRadius; } }

            public float DashTimeOut { get { return _dashTimeOut; } }

            public float DashSpeed { get { return _dashSpeed; } }

            public float DashFloatingTimeout { get { return _dashFloatingTimeout; } }

            public float DashFloatingGravityScale { get { return _dashFloatingGravityScale; } }

            public float CheckGroundRadius { get { return _checkGroundRadius; } }

            public float CheckWallRadius { get { return _checkWallRadius; } }

            public LayerMask WhatIsGround { get { return _whatIsGround; } }

            public List<Vector2> AllPaths { get { return _allPaths; } }

            public float DeadTimer { get { return _deadTimer; } }

            public Vector2 LastCheckpoint { get { return _lastCheckpoint; } }

            public float RecoilTimer { get { return _recoilTimer; } }

            public float FreeFallGravity { get { return _freeFallGravity;  } }

            public float MaxDeformedScale { get { return _maxDeformedScale; } }

            public float JumpForce { get { return _jumpForce; } }

            public float JumpGravity { get { return _jumpGravity; } }

            public float JumpCoyoteTiming { get { return _jumpCoyoteTiming; } }

            public float FloatGravity { get { return _floatGravity; } }

            public float StartingWallSlideGravity { get { return _startingWallSlideGravity; } }

            public float FinalWallSlideGravity { get { return _finalWallSlideGravity; } }

            public float TimeToReachFinalGravity { get { return _timeToReachFinalGravity; } }

            public float StartingFallSpeed { get { return _startingFallSpeed; } }

            public float WallJumpDuration { get { return _wallJumpDuration; } }

            public float WallJumpGravity { get { return _wallJumpGravity; } }

            public float WallJumpForceHorizontal { get { return _wallJumpForceHorizontal; } }

            public float WallJumpForceVertical { get { return _wallJumpForceVertical;  } }

            public float WallJumpCoyoteTiming { get { return _wallJumpCoyoteTiming; } }

            public float ScaleReductionModifier { get { return _scaleReductionModifier; } }

            public float FreeFallMaxFallSpeed => _freeFallMaxFallSpeed;

            public float WallSlideMaxFallSpeed => _wallSlideMaxFallSpeed;

            public float FloatingGravityScaleIntoWall => _floatingGravityScaleIntoWall;
        }
    }
}



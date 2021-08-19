using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        [RequireComponent(typeof(PlayerController))]
        [RequireComponent(typeof(BoxCollider2D))]
        [RequireComponent(typeof(PlayerInputManager))]
        [RequireComponent(typeof(PlayerCollisions))]
        [RequireComponent(typeof(LineRenderer))]

        public class Player : MonoBehaviour, IPlayer
        {
            [SerializeField]
            private PlayerData _playerData;

            private PlayerCollisions _playerCollisions;
            private PlayerArrowRendering _playerArrowRendering;
            [SerializeField] private PlayerAnimations _playerAnimations;

            [SerializeField]
            private SpriteRenderer _sr;

            public IPlayerStateMachine StateMachine { get; private set; }
            public IPlayerInputManager InputManager { get; private set; }
            public IPlayerController PlayerController { get; private set; }
            public SpriteRenderer Sr { get => _sr; private set => _sr = value; }
            public BoxCollider2D Bc { get; private set; }
            public CircleCollider2D DashCollider { get; private set; }

            [SerializeField]
            private GameObject _deathParticles;

            [SerializeField] private Transform _playerSet;

            #region Auxiliary Variables
            private const float GRAVITY = -9.8f;

            [SerializeField] private float _universalGravityScale;
            private float _gravityScale;

            [SerializeField] private float _maxFallSpeed;

            private Vector2 _velocity;
            public Vector2 Velocity { get { return _velocity; } set { _velocity = value; } }

            //[SerializeField]
            //private Vector2 _checkpoint;

            [SerializeField]
            private Camera _mainCamera;

            [SerializeField]
            private Transform _groundCheckOne;

            [SerializeField]
            private Transform _groundCheckTwo;

            [SerializeField]
            private Transform _wallCheckOne;

            [SerializeField]
            private Transform _wallCheckTwo;

            private Vector3 _dashStart;
            private Vector3 _dashFinish;

            private Vector2 _auxVector2;
            private Vector3 _auxVector3;

            private bool _isFacingRight = true;

            private bool _wallFlipped = false;

            public bool IsFacingRight { get => _isFacingRight; set => _isFacingRight = value; }
            public bool WallFlipped { get => _wallFlipped; set => _wallFlipped = value; }
            public bool IsDead { get; set; } = false;

            #endregion

            #region Unity Callback Functions

            private void Start()
            {
                GetComponents();
                SetRigidbody();
                StateMachine.Initialize();
            }

            private void Update()
            {
                StateMachine.CurrentState.LogicUpdate();
            }

            private void FixedUpdate()
            {
                StateMachine.CurrentState.PhysicsUpdate();
            }

            private void GetComponents()
            {
                PlayerController = GetComponent<PlayerController>();
                Bc = GetComponent<BoxCollider2D>();
                DashCollider = GetComponent<CircleCollider2D>();
                InputManager = GetComponent<PlayerInputManager>();
                _playerCollisions = GetComponent<PlayerCollisions>();

                StateMachine = new PlayerStateMachine(this, _playerData);
                _playerArrowRendering = new PlayerArrowRendering(GetComponent<LineRenderer>());
            }

            private void SetRigidbody()
            {
                _gravityScale = _playerData.PlayerPhysicsData.StandardGravity;
                _maxFallSpeed = _playerData.PlayerPhysicsData.StandardMaxFallSpeed;
            }
            #endregion

            public void Gravity()
            {
                if(_velocity.y >= _maxFallSpeed)
                {
                    _velocity.y += Time.deltaTime * GRAVITY * _gravityScale * _universalGravityScale;
                }
                else
                {
                    _velocity.y = _maxFallSpeed;
                }
            }

            public void Move(Vector2 vector)
            {
                PlayerController.Move(vector);
            }

            public void MoveX(float direction, float velocity)
            {
                PlayerController.MoveX(direction, velocity);
            }

            public void MoveY(float direction, float velocity)
            {
                PlayerController.MoveY(direction, velocity);
            }

            public void Dash(float velocity)
            {
                _auxVector2 = _dashFinish - _auxVector3;
                _dashFinish = _auxVector2;
                _auxVector2.Set(_dashFinish.normalized.x, _dashFinish.normalized.y);
                Velocity = _auxVector2 * velocity;
                Velocity = Vector2.ClampMagnitude(Velocity, velocity);
            }

            public void SetDashVectors(Vector3 startPos, Vector3 finalPos)
            {
                if(finalPos == Vector3.zero)
                {
                    finalPos = _isFacingRight ? Vector3.right : Vector3.left;
                }

                Vector3 direction = (finalPos - startPos).normalized;

                _dashStart = transform.position;
                _dashFinish = transform.position + direction;

                _auxVector3 = transform.position;
            }

            public void SetArrowRendering()
            {
                Vector2 aux1 = _dashStart;
                Vector2 aux2 = _dashFinish;

                Vector2 direction = aux2 - aux1;

                _playerArrowRendering.RenderArrow(aux1, aux2 + direction * 2);
            }

            public void SetPosition(Vector2 position)
            {
                transform.position = position;
            }

            public void SetGravityScale(float gravity)
            {
                _gravityScale = gravity;
            }

            public void SetVelocity(Vector2 velocity)
            {
                _velocity = velocity;
            }

            public void ResetGravity()
            {
                _velocity.y = 0f;
            }

            public void SetMaxFallSpeed(float newFallSpeed)
            {
                _maxFallSpeed = newFallSpeed;
            }

            public void SetScale(Vector2 scale)
            {
                float facingDirection = Mathf.Sign(transform.localScale.x);

                scale = new Vector2(scale.x * facingDirection, scale.y);
                transform.localScale = scale;
            }

            public void SetActivePhysicsCollider(bool set)
            {
                Bc.enabled = set;
            }

            public void SetDashCollider(bool set)
            {
                DashCollider.enabled = set;
            }

            public void SetActiveSpriteRenderer(bool set)
            {
                Sr.enabled = set;
            }

            public bool CheckIsGrounded()
            {
                return Physics2D.OverlapCircle(_groundCheckOne.position, _playerData.CheckGroundRadius, _playerData.WhatIsGround)
                    || Physics2D.OverlapCircle(_groundCheckTwo.position, _playerData.CheckGroundRadius, _playerData.WhatIsGround);
            }

            public bool CheckIsFreeFalling()
            {
                return Velocity.y < 0f;
            }

            public bool CheckIsTouchingWallLeft()
            {
                return Physics2D.OverlapCircle(_wallCheckTwo.position, _playerData.CheckWallRadius, _playerData.WhatIsGround);
            }

            public bool CheckIsTouchingWallRight()
            {
                return Physics2D.OverlapCircle(_wallCheckOne.position, _playerData.CheckWallRadius, _playerData.WhatIsGround);
            }

            public bool CheckIsTouchingNode()
            {
                return _playerCollisions.IsTouchingNode;
            }

            public bool CheckIsTouchingRail()
            {
                return _playerCollisions.IsTouchingRail;
            }

            public bool CheckHasReachedPoint(Vector2 point)
            {
                //Debug.Log("Transform: (" + transform.position.x + " , " + transform.position.y + ") , Point: (" + point.x + " , " + point.y + ")");
                return (Vector2)transform.position == point;
            }


            public Collider2D GetNodeInfo()
            {
                return _playerCollisions.NodeInfo;
            }

            public RailController GetRailInfo()
            {
                return _playerCollisions.FirstRail;
            }

            public Vector2 GetCurrentVelocity()
            {
                //return Rb.velocity;
                return Vector2.zero;
            }

            public void DeactivateArrowRendering()
            {
                _playerArrowRendering.DerenderArrow();
            }

            public void ResetPosition()
            {
                // Instantiate particles
                // Move player to last _checkpoint (but here we will have only one _checkpoint, so skip)
                //transform.position = _checkpoint;
                transform.position = _playerData.LastCheckpoint;
                // Reset objects (but here they are immutable so skip)
            }

            public void InstantiateDeathParticles()
            {
                Instantiate(_deathParticles, transform.position, Quaternion.identity);
            }

            public void CheckIfShouldFlip(float direction)
            {
                bool shouldFaceRight = direction > 0f && !_isFacingRight;
                bool shouldFaceLeft = direction < 0f && _isFacingRight;

                if(shouldFaceLeft || shouldFaceRight)
                {
                    Flip();
                }
            }

            public void SetAnimationBool(string name, bool value)
            {
                _playerAnimations.SetAnimationBool(name, value);
            }

            public bool GetAnimationBool(string name)
            {
                return _playerAnimations.GetAnimationBool(name);
            }

            public void Flip()
            {
                _isFacingRight = !_isFacingRight;
                _playerAnimations.Flip();
            }

            public void FlipY()
            {
                _playerAnimations.FlipY();
            }

            public void SetAnimationInt(string name, int value)
            {
                _playerAnimations.SetAnimationInt(name, value);
            }

            public void ActivateInput(bool set)
            {
                InputManager.Disabled = set;
            }

            public void ResetJumpInputTimer()
            {
                InputManager.ResetJumpTimer();
            }
        }

    }
}


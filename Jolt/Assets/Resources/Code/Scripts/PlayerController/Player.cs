using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        [RequireComponent(typeof(SpriteRenderer))]
        [RequireComponent(typeof(CircleCollider2D))]
        [RequireComponent(typeof(PlayerInputManager))]
        [RequireComponent(typeof(PlayerCollisions))]
        [RequireComponent(typeof(LineRenderer))]

        public class Player : MonoBehaviour, IPlayer
        {
            #region Components
            [SerializeField]
            private PlayerData _playerData;

            private PlayerCollisions _playerCollisions;
            private PlayerArrowRendering _playerArrowRendering;

            public IPlayerStateMachine StateMachine { get; private set; }
            public IPlayerInputManager InputManager { get; private set; }

            //public Rigidbody2D Rb { get; private set; }
            public SpriteRenderer Sr { get; private set; }
            public CircleCollider2D Cc { get; private set; }

            [SerializeField]
            private GameObject _deathParticles;
            #endregion

            #region Auxiliary Variables
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
                //Rb = GetComponent<Rigidbody2D>();
                Sr = GetComponent<SpriteRenderer>();
                Cc = GetComponent<CircleCollider2D>();
                InputManager = GetComponent<PlayerInputManager>();
                _playerCollisions = GetComponent<PlayerCollisions>();

                StateMachine = new PlayerStateMachine(this, _playerData);
                _playerArrowRendering = new PlayerArrowRendering(GetComponent<LineRenderer>());
            }

            private void SetRigidbody()
            {
                //Rb.gravityScale = _playerData.PlayerPhysicsData.StandardGravity;
                //Rb.drag = _playerData.PlayerPhysicsData.StandardLinearDrag;
            }

            private void OnDrawGizmos()
            {
                //Vector2 center = new Vector2(1f, 0);
                //float radius = 4;

                //float x, y;
                //for (int i = 0; i <= 50; i += 1)
                //{
                //    x = radius * Mathf.Cos(Mathf.Rad2Deg * i) + center.x;
                //    y = radius * Mathf.Sin(Mathf.Rad2Deg * i) + center.y;

                //    Gizmos.DrawSphere(new Vector2(x, y), 0.1f);
                //}
            }
            #endregion

            #region Set Functions
            public void MoveTowardsVector(Vector2 vector, float velocity)
            {
                //Debug.Log("Transform: (" + transform.position.x + " , " + transform.position.y + ") , Point: (" + v.x + " , " + v.y + ")");
                _auxVector2.Set(vector.x, vector.y);
                transform.position = Vector2.MoveTowards(transform.position, _auxVector2, velocity * Time.deltaTime);
            }

            public void MoveX(float direction, float velocity)
            {
                //_auxVector2.Set(velocity, Rb.velocity.y);
                //Rb.velocity = _auxVector2;
            }

            public void MoveY(float direction, float velocity)
            {
                //_auxVector2.Set(Rb.velocity.x, velocity);
                //Rb.velocity = _auxVector2;
            }

            public void Dash(Vector3 startPos, Vector3 finalPos, float velocity)
            {
                SetDashVectors(startPos, finalPos);

                _auxVector2 = _dashFinish - _auxVector3;
                _dashFinish = _auxVector2;
                _auxVector2.Set(_dashFinish.normalized.x, _dashFinish.normalized.y);
                //Rb.velocity = _auxVector2 * velocity;
                //Rb.velocity = Vector2.ClampMagnitude(Rb.velocity, velocity);
            }

            private void SetDashVectors(Vector3 startPos, Vector3 finalPos)
            {
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
                //Rb.gravityScale = gravity;
            }

            public void SetDrag(float drag)
            {
                //Rb.drag = drag;
            }

            public void SetScale(Vector2 scale)
            {
                transform.localScale = scale;
            }

            public void SetActivePhysicsCollider(bool set)
            {
                Cc.enabled = set;
            }

            public void SetActiveSpriteRenderer(bool set)
            {
                Sr.enabled = set;
            }
            #endregion

            #region Check Functions
            public bool CheckIsGrounded()
            {
                return Physics2D.OverlapCircle(_groundCheckOne.position, _playerData.CheckGroundRadius, _playerData.WhatIsGround)
                    || Physics2D.OverlapCircle(_groundCheckTwo.position, _playerData.CheckGroundRadius, _playerData.WhatIsGround);
            }

            public bool CheckIsFreeFalling()
            {
                //return Rb.velocity.y < 0f;
                return false;
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

            #endregion

            #region Get Functions
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
            #endregion

            #region Other Functions
            public void DeactivateArrowRendering()
            {
                _playerArrowRendering.DerenderArrow();
            }

            public void ResetPosition()
            {
                // Instantiate particles
                // Move player to last _checkpoint (but here we will have only one _checkpoint, so skip)
                //transform.position = _checkpoint;
                transform.position = Vector2.zero;
                // Reset objects (but here they are immutable so skip)
            }

            public void InstantiateDeathParticles()
            {
                Instantiate(_deathParticles, transform.position, Quaternion.identity);
            }

            


            #endregion
        }

    }
}


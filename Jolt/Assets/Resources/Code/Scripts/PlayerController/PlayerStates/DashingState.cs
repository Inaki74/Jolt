﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class DashingState : AliveState, ICanDash
            {
                private const float DASH_COLLIDER_OFFSET = 0.25f;

                protected override Color AssociatedColor => Color.cyan;
                protected override string AnimString => PlayerAnimations.Constants.DASH_BOOL;
                //public override bool Flippable => false;

                public Node LastNode { private get; set; } = null;
                public bool WasInNode { private get; set; } = false;

                private bool _isTouchingWallLeft;
                private bool _isTouchingWallRight;

                private bool _isNotLastNode;
                
                private bool _isGrounded;
                private Vector2 _moveInput;
                private Vector3 _dashFinalPoint;
                private float _currentTime;
                private bool _isTouchingNode;
                private bool _isTouchingRail;

                private bool _flippedY;
                
                private int _amountOfDashes;

                private bool _playOnce;

                public DashingState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                    ResetAmountOfDashes();
                }

                public override void Enter()
                {
                    //Debug.Break();
                    base.Enter();

                    _moveInput = _player.InputManager.MovementVector;
                    _dashFinalPoint = _player.InputManager.FinalDashPoint;
                    _playOnce = true;
                    _player.SetGravityScale(0f);
                    _player.Velocity = Vector2.zero;

                    DecreaseAmountOfDashes();

                    SetAnimationsEntry();

                    FlipYIfDashingDown();
                }

                public override void Exit()
                {
                    base.Exit();

                    WasInNode = false;

                    _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                    //_player.Velocity = Vector2.zero;

                    _player.SetDashColliderOffset(Vector2.zero);

                    ResetAnimationVariables();

                    UnflipIfFlippedY();
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _currentTime = Time.time;

                    _isGrounded = _player.CheckIsGrounded();
                    _isTouchingNode = _player.CheckIsTouchingNode();
                    _isTouchingRail = _player.CheckIsTouchingRail();
                    _isTouchingWallLeft = _player.CheckIsTouchingWallLeft();
                    _isTouchingWallRight = _player.CheckIsTouchingWallRight();
                    bool isTouchingWall = _isTouchingWallLeft || _isTouchingWallRight;
                    bool onLeftWallAndMovingTowardsIt = _isTouchingWallLeft && _moveInput.x < 0f;
                    bool onRightWallAndMovingTowardsIt = _isTouchingWallRight && _moveInput.x > 0f;

                    if (CheckIsAdmittableToGetIntoNode())
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.InNodeState);
                        return false;
                    }

                    if (_isTouchingRail)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.InRailState);
                        return false;
                    }

                    if (isTouchingWall && _dashFinalPoint.x == 0f && (onLeftWallAndMovingTowardsIt || onRightWallAndMovingTowardsIt))
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.WallSlideState);
                        return false;
                    }

                    if (_currentTime - _enterTime > _playerData.DashTimeOut)
                    {
                        if (_isGrounded)
                        {
                            if (_moveInput.x != 0)
                            {
                                _stateMachine.ScheduleStateChange(_stateMachine.MoveState);
                                return false;
                            }
                            else
                            {
                                _stateMachine.ScheduleStateChange(_stateMachine.IdleState);
                                return false;
                            }
                        }
                        else
                        {
                            _stateMachine.ScheduleStateChange(_stateMachine.DashFloatingState);
                            return false;
                        }
                    }

                    if(_isGrounded && _currentTime - _enterTime > _playerData.DashTimeOut / 10) // Consider triple dash
                    {
                        ResetAmountOfDashes();
                    }

                    return true;
                }

                protected override void PlayerControlAction()
                {
                    base.PlayerControlAction();

                    if (_playOnce)
                    {
                        PrepareDash();

                        _playOnce = false;
                    }

                    _player.Dash(_playerData.DashSpeed * _playerData.DashTimeOut, _playerData.DashSpeed);
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                }

                public override string ToString()
                {
                    return "DashingState";
                }

                public void ResetLastnode()
                {
                    LastNode = null;
                }

                public bool CanDash()
                {
                    return _amountOfDashes > 0;
                }

                public void ResetAmountOfDashes()
                {
                    _amountOfDashes = _playerData.AmountOfDashes;
                }

                public void DecreaseAmountOfDashes()
                {
                    _amountOfDashes--;
                }

                private void PrepareDash()
                {
                    bool onLeftWallAndMovingTowardsIt = _isTouchingWallLeft && _moveInput.x < 0f;
                    bool onRightWallAndMovingTowardsIt = _isTouchingWallRight && _moveInput.x > 0f;

                    Vector3 newFinalDirection = _player.InputManager.FinalDashPoint;

                    newFinalDirection = onLeftWallAndMovingTowardsIt ? Vector3.right : newFinalDirection;
                    newFinalDirection = onRightWallAndMovingTowardsIt ? Vector3.left : newFinalDirection;

                    if (onLeftWallAndMovingTowardsIt || onRightWallAndMovingTowardsIt)
                    {
                        _player.Flip();
                        _player.WallFlipped = false;

                        if (_dashFinalPoint.y > 0f)
                        {
                            newFinalDirection = new Vector3(newFinalDirection.x, 1f, 0f);
                        }
                        else if (_dashFinalPoint.y < 0f)
                        {
                            newFinalDirection = new Vector3(newFinalDirection.x, -1f, 0f);
                        }
                    }

                    _player.SetDashVectors(_player.InputManager.InitialDashPoint, newFinalDirection);

                    float newFinalDirectionDirectionY = newFinalDirection.y == 0f ? 0f : Mathf.Sign(newFinalDirection.y);
                    float newFinalDirectionDirectionX = newFinalDirection.x == 0f ? 0f : Mathf.Sign(newFinalDirection.x);

                    _player.SetDashColliderOffset(new Vector2(newFinalDirectionDirectionX * DASH_COLLIDER_OFFSET, newFinalDirectionDirectionY * DASH_COLLIDER_OFFSET));
                }

                private bool CheckIsAdmittableToGetIntoNode()
                {
                    _isNotLastNode = false;
                    Collider2D lastNodeCollider = _player.GetNodeInfo();

                    if (lastNodeCollider != null && LastNode != null)
                    {
                        Node currentNode = _player.GetNodeInfo().GetComponent<Node>();

                        _isNotLastNode = currentNode.GetInstanceID() != LastNode.GetInstanceID();
                    }

                    if (LastNode == null)
                    {
                        _isNotLastNode = true;
                    }

                    if (WasInNode)
                    {
                        if (!_isTouchingNode)
                        {
                            _stateMachine.DashingState.ResetLastnode();
                            WasInNode = false;
                        }
                    }

                    return _isTouchingNode && _isNotLastNode && !WasInNode;
                }

                private void SetAnimationsEntry()
                {
                    if (_dashFinalPoint.x == 0f && _dashFinalPoint.y == 0f)
                    {
                        _player.SetAnimationInt(PlayerAnimations.Constants.DASHX_INT, 1);
                        _player.SetAnimationInt(PlayerAnimations.Constants.DASHY_INT, 0);
                    }
                    else
                    {
                        _player.SetAnimationInt(PlayerAnimations.Constants.DASHX_INT, (int)_dashFinalPoint.x);
                        _player.SetAnimationInt(PlayerAnimations.Constants.DASHY_INT, (int)_dashFinalPoint.y);
                    }
                }

                private void ResetAnimationVariables()
                {
                    _player.SetAnimationInt(PlayerAnimations.Constants.DASHX_INT, 0);
                    _player.SetAnimationInt(PlayerAnimations.Constants.DASHY_INT, 0);
                }

                private void FlipYIfDashingDown()
                {
                    if (_dashFinalPoint.y < 0f)
                    {
                        _flippedY = true;
                        _player.FlipY();
                    }
                }

                private void UnflipIfFlippedY()
                {
                    if (_flippedY)
                    {
                        _flippedY = false;
                        _player.FlipY();
                    }
                }
            }
        }
    }
}

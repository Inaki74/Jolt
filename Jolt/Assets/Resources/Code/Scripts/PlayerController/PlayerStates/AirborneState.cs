﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class AirborneState : FullControlState
            {
                protected override Color AssociatedColor => Color.red;

                private bool _isGrounded;
                private bool _isMoving;
                private bool _isTouchingWallLeft;
                private bool _isTouchingWallRight;
                private float _freefallDeformedScaleX;
                private bool a = false;

                public AirborneState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    a = false;
                    _freefallDeformedScaleX = 1f;
                }

                public override void Exit()
                {
                    base.Exit();
                    Debug.Log("AAAA");
                    a = true;
                    _player.SetScale(Vector2.one);
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _isMoving = _moveInput.x != 0;
                    _isGrounded = _player.CheckIsGrounded();
                    _isTouchingWallLeft = _player.CheckIsTouchingWallLeft();
                    _isTouchingWallRight = _player.CheckIsTouchingWallRight();
                    bool isTouchingWall = _isTouchingWallLeft || _isTouchingWallRight;
                    bool isMovingRight = _moveInput.x > 0f;
                    bool isMovingLeft = _moveInput.x < 0f;

                    // If it hits ground -> recoil
                    
                    if (_isGrounded)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.IdleState);
                        return false;
                    }

                    if (isTouchingWall)
                    {
                        if((_isTouchingWallLeft && isMovingLeft) ||
                            (_isTouchingWallRight && isMovingRight))
                        {
                            _stateMachine.ScheduleStateChange(_stateMachine.WallSlideState);
                            return false;
                        }

                        _stateMachine.ScheduleStateChange(_stateMachine.WallAirborneState);
                        return false;
                    }

                    return true;
                }

                protected override void PlayerControlAction()
                {
                    base.PlayerControlAction();

                    if (a)
                    {
                        Debug.Log("PPPPPPPPP");
                    }

                    Freefall();

                    // TODO: WTF is this.
                    //if (_isMoving)
                    //{
                    //    if (_stateMachine.LastState == "ExitRailState")
                    //    {
                    //        _player.SetMovementXByForce(Vector2.right, _playerData.MovementSpeed * _moveInput.x);
                    //    }
                    //    else
                    //    {
                    //        _player.SetRigidbodyVelocityX(_playerData.MovementSpeed * _moveInput.x);
                    //    }
                    //}
                    //else
                    //{
                    //    if (_stateMachine.LastState != "ExitRailState")
                    //    {
                    //        _player.SetRigidbodyVelocityX(0f);
                    //    }
                    //}
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                }

                private void Freefall()
                {
                    if (_moveInput.y < 0f)
                    {
                        if (_freefallDeformedScaleX > _playerData.MaxDeformedScale)
                        {
                            _freefallDeformedScaleX -= 0.01f;
                        }

                        Vector2 newScale = new Vector2(_freefallDeformedScaleX, 1f);
                        _player.SetScale(newScale);

                        _player.SetGravityScale(_playerData.FreeFallGravity);
                    }
                    else
                    {
                        _freefallDeformedScaleX = 1f;
                        _player.SetScale(Vector2.one);
                        _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                    }
                }
            }
        }
    }
}



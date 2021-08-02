﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class WallJumpState : AliveState
            {
                protected override Color AssociatedColor => Color.green;

                private bool _jumpHeld;
                private bool _forceApplied;
                private float _currentTime;
                private bool _reachedPeak;

                public Vector2 JumpDirection { private get; set; }

                public WallJumpState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _forceApplied = false;

                    _player.SetGravityScale(_playerData.WallJumpGravity);
                    _player.SetDrag(_playerData.WallJumpDrag);
                    //_player.SetRigidbodyVelocityX(0f);
                }

                public override void Exit()
                {
                    base.Exit();

                    JumpDirection = Vector2.zero;
                    _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                    _player.SetDrag(_playerData.PlayerPhysicsData.StandardLinearDrag);
                }

                public override bool LogicUpdate()
                {
                    bool continueExecution = base.LogicUpdate();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _currentTime = Time.time;

                    //_isTouchingWall = _player.CheckIsTouchingWallLeft() || _player.CheckIsTouchingWallRight();
                    bool timeout = _currentTime - _enterTime > 0.25f;
                    _reachedPeak = _player.CheckIsFreeFalling();
                    _jumpHeld = _player.InputManager.JumpHeld;

                    if (_forceApplied)
                    {
                        if(!_jumpHeld || _reachedPeak || timeout)
                        {
                            _stateMachine.ChangeState(_stateMachine.AirborneState);
                            return false;
                        }
                    }

                    return true;
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();

                    if (!_forceApplied)
                    {
                        _player.SetRigidbodyVelocityY(0f);
                        WallJump();
                        _forceApplied = true;
                    }
                }

                public override string ToString()
                {
                    return "IdleState";
                }

                private void WallJump()
                {
                    float speed = _playerData.WallJumpForceHorizontal;
                    float horizontalForce = 1;
                    float verticalForce = horizontalForce * _playerData.WallJumpForceVerticalRatioWithHorizontal;

                    Vector2 impulseDirection = new Vector2(horizontalForce, verticalForce);
                    Vector2 side = new Vector2(JumpDirection.x, Mathf.Abs(JumpDirection.x));
                    impulseDirection = impulseDirection * side;

                    _player.SetMovementByImpulse(impulseDirection, speed);
                }
            }
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class WallSlideState : AliveState
            {
                protected override Color AssociatedColor => Color.blue;

                private bool _isStartingDash;
                private bool _isGrounded;
                private Vector2 _moveInput;
                private bool _jumpInput;
                private bool _isTouchingWallLeft;
                private bool _isTouchingWallRight;
                private bool _wasTouchingWallLeft;
                private bool _wasTouchingWallRight;
                private bool _canDash;

                public WallSlideState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    //_player.SetRigidbodyVelocityY(_player.GetCurrentVelocity().y / _playerData.InverseMultiplierOfFallSpeed);
                    _player.SetRigidbodyVelocityY(_playerData.InverseMultiplierOfFallSpeed);
                    _player.SetGravityScale(_playerData.WallSlideGravity);
                    _player.SetDrag(_playerData.WallSlideDrag);
                }

                public override void Exit()
                {
                    base.Exit();

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

                    _moveInput = _player.InputManager.MovementVector;
                    _isGrounded = _player.CheckIsGrounded();
                    _isStartingDash = _player.InputManager.DashBegin;
                    _jumpInput = _player.InputManager.JumpPressed;
                    _canDash = _stateMachine.PreDashState.CanDash();

                    _isTouchingWallLeft = _player.CheckIsTouchingWallLeft();
                    _isTouchingWallRight = _player.CheckIsTouchingWallRight();
                    bool isTouchingWall = _isTouchingWallLeft || _isTouchingWallRight;
                    bool isMovingRight = _moveInput.x > 0f;
                    bool isMovingLeft = _moveInput.x < 0f;

                    if (_isGrounded)
                    {
                        _stateMachine.ChangeState(_stateMachine.IdleState);
                        return false;
                    }

                    if (_isStartingDash && _canDash)
                    {
                        _stateMachine.ChangeState(_stateMachine.PreDashState);
                        return false;
                    }

                    if((_isTouchingWallLeft && !isMovingLeft) ||
                       (_isTouchingWallRight && !isMovingRight) ||
                       (!isTouchingWall))
                    {
                        if (_wasTouchingWallLeft)
                        {
                            _stateMachine.WallJumpState.JumpDirection = Vector2.right;
                        }
                        if (_wasTouchingWallRight)
                        {
                            _stateMachine.WallJumpState.JumpDirection = Vector2.left;
                        }

                        _stateMachine.ChangeState(_stateMachine.CoyoteWallJumpState);
                        return false;
                    }

                    if (_jumpInput)
                    {
                        if (_isTouchingWallLeft)
                        {
                            _stateMachine.WallJumpState.JumpDirection = Vector2.right;
                        }
                        if (_isTouchingWallRight)
                        {
                            _stateMachine.WallJumpState.JumpDirection = Vector2.left;
                        }

                        _stateMachine.ChangeState(_stateMachine.WallJumpState);
                        return false;
                    }

                    _wasTouchingWallLeft = _isTouchingWallLeft;
                    _wasTouchingWallRight = _isTouchingWallRight;

                    return true;
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();

                    _player.SetRigidbodyVelocityX(_moveInput.x * _playerData.MovementSpeed);
                }

                public override string ToString()
                {
                    return "IdleState";
                }
            }
        }
    }
}



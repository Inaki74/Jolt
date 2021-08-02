using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class CoyoteWallJumpState : PlayerState
            {
                protected override Color AssociatedColor => Color.magenta;

                public Vector2 WallSide { private get; set; }

                private Vector2 _moveInput;
                private bool _jumpPressed;
                private float _currentTime;
                private bool _isTouchingWallLeft;
                private bool _isTouchingWallRight;

                public CoyoteWallJumpState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                }

                public override void Exit()
                {
                    base.Exit();
                }

                public override bool LogicUpdate()
                {
                    bool continueExecution = base.LogicUpdate();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _currentTime = Time.time;

                    _moveInput = _player.InputManager.MovementVector;
                    _jumpPressed = _player.InputManager.JumpPressed;
                    bool timeout = _currentTime - _enterTime > _playerData.WallJumpCoyoteTiming;
                    _isTouchingWallLeft = _player.CheckIsTouchingWallLeft();
                    _isTouchingWallRight = _player.CheckIsTouchingWallRight();
                    bool isTouchingWall = _isTouchingWallLeft || _isTouchingWallRight;

                    if (_jumpPressed)
                    {
                        //if (_isTouchingWallLeft)
                        //{
                        //    _stateMachine.WallJumpState.JumpDirection = Vector2.right;
                        //}

                        //if (_isTouchingWallRight)
                        //{
                        //    _stateMachine.WallJumpState.JumpDirection = Vector2.left;
                        //}

                        //if (!isTouchingWall)
                        //{
                        //    _stateMachine.WallJumpState.JumpDirection = new Vector2(_moveInput.x, 0f);
                        //}

                        _stateMachine.ChangeState(_stateMachine.WallJumpState);
                        return false;
                    }

                    if (timeout)
                    {
                        _stateMachine.ChangeState(_stateMachine.AirborneState);
                        return false;
                    }

                    return true;
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                    _player.SetRigidbodyVelocityX(_playerData.MovementSpeed * _moveInput.x);
                }

                public override string ToString()
                {
                    return "IdleState";
                }
            }
        }
    }
}
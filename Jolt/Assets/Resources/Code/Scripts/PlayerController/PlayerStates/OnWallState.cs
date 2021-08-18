using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public abstract class OnWallState : FullControlState
            {
                protected override Color AssociatedColor => Color.magenta;

                protected bool _isGrounded;
                public override bool Flippable => false;

                protected bool _enteredTouchingRightWall;
                protected bool _isTouchingWallLeft;
                protected bool _isTouchingWallRight;

                public OnWallState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    //_player.SetRigidbodyVelocityX(0f);

                    //_player.ResetJumpInputTimer();

                    if (!_player.WallFlipped)
                    {
                        _player.Flip();
                        _player.WallFlipped = true;
                    }
                }

                public override void Exit()
                {
                    base.Exit();

                    if (_player.WallFlipped && _stateMachine.NextState.Flippable)
                    {
                        _player.Flip();
                        _player.WallFlipped = false;
                    }
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _isGrounded = _player.CheckIsGrounded();
                    _isTouchingWallLeft = _player.CheckIsTouchingWallLeft();
                    _isTouchingWallRight = _player.CheckIsTouchingWallRight();
                    bool isTouchingWall = _isTouchingWallLeft || _isTouchingWallRight;

                    if (_isTouchingWallLeft)
                    {
                        _stateMachine.WallJumpState.JumpDirection = Vector2.right;
                    }

                    if (_isTouchingWallRight)
                    {
                        _stateMachine.WallJumpState.JumpDirection = Vector2.left;
                    }

                    if (_jumpPressed)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.WallJumpState);
                        return false;
                    }

                    if (!isTouchingWall)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.CoyoteWallJumpState);
                        return false;
                    }

                    return true;
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                }

            }
        }
    }
}
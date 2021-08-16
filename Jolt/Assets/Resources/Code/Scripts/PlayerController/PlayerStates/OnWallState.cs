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
                protected bool _isTouchingWallLeft;
                protected bool _isTouchingWallRight;
                protected override bool _flippable => false;

                protected bool _enteredTouchingRightWall;

                public OnWallState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    //_player.SetRigidbodyVelocityX(0f);

                    _player.Flip();
                }

                public override void Exit()
                {
                    base.Exit();

                    //_player.Flip();
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

                    if (_jumpPressed)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.WallJumpState);
                        return false;
                    }

                    if (_isTouchingWallLeft)
                    {
                        _stateMachine.WallJumpState.JumpDirection = Vector2.right;
                    }

                    if (_isTouchingWallRight)
                    {
                        _stateMachine.WallJumpState.JumpDirection = Vector2.left;
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
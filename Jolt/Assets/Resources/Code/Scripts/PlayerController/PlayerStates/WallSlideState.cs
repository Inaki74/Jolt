using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class WallSlideState : OnWallState
            {
                protected override Color AssociatedColor => Color.blue;

                public WallSlideState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();
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

                    _isGrounded = _player.CheckIsGrounded();
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

                    if ((_isTouchingWallLeft && !isMovingLeft) ||
                        (_isTouchingWallRight && !isMovingRight) ||
                        (!isTouchingWall))
                    {
                        _stateMachine.ChangeState(_stateMachine.CoyoteWallJumpState);
                        return false;
                    }

                    return true;
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                }

                protected override void PhysicsFirstStep()
                {
                    base.PhysicsFirstStep();

                    _player.SetRigidbodyVelocityY(_playerData.InverseMultiplierOfFallSpeed);
                    _player.SetGravityScale(_playerData.WallSlideGravity);
                    _player.SetDrag(_playerData.WallSlideDrag);
                }

                public override string ToString()
                {
                    return "IdleState";
                }
            }
        }
    }
}



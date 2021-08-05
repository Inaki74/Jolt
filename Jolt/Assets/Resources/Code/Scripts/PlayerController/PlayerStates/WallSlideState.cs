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
                    //_player.SetRigidbodyVelocityY(_playerData.InverseMultiplierOfFallSpeed);
                    _player.SetGravityScale(_playerData.WallSlideGravity);
                    _player.SetDrag(_playerData.WallSlideDrag);
                }

                public override void Exit()
                {
                    base.Exit();

                    _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                    _player.SetDrag(_playerData.PlayerPhysicsData.StandardLinearDrag);
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    bool isMovingRight = _moveInput.x > 0f;
                    bool isMovingLeft = _moveInput.x < 0f;

                    if (_isGrounded)
                    {
                        _stateMachine.ChangeState(_stateMachine.IdleState);
                        return false;
                    }

                    if ((_isTouchingWallLeft && !isMovingLeft) ||
                        (_isTouchingWallRight && !isMovingRight))
                    {
                        _stateMachine.ChangeState(_stateMachine.WallAirborneState);
                        return false;
                    }

                    return true;
                }

                protected override void PlayerControlAction()
                {
                    base.PlayerControlAction();
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                }
            }
        }
    }
}



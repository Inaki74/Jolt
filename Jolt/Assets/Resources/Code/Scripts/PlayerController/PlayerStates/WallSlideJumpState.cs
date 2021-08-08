using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class WallSlideJumpState : OnWallState
            {
                protected override Color AssociatedColor => Color.gray;

                public bool ForceApplied { private get; set; }

                public WallSlideJumpState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    ForceApplied = false;
                    _player.SetGravityScale(_playerData.JumpGravity);
                }

                public override void Exit()
                {
                    base.Exit();

                    _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    if (ForceApplied && !_isGrounded)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.WallSlideFloatingState);
                        return false;
                    }

                    if (ForceApplied && _isGrounded)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.IdleState);
                        return false;
                    }

                    return true;
                }

                protected override void PlayerControlAction()
                {
                    base.PlayerControlAction();
                    if (!ForceApplied)
                    {
                        _player.Velocity = new Vector2(_player.Velocity.x, _playerData.JumpForce);
                        //_player.SetRigidbodyVelocityY(_playerData.JumpForce);
                        //_player.SetMovementByImpulse(Vector2.up, _playerData.JumpForce);
                        ForceApplied = true;
                    }
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                }
            }
        }
    }
}



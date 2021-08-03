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
                private bool _isGrounded;

                public WallSlideJumpState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                }

                public override void Exit()
                {
                    base.Exit();

                    ForceApplied = false;
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

                    if (ForceApplied && !_isGrounded)
                    {
                        _stateMachine.ChangeState(_stateMachine.WallSlideFloatingState);
                        return false;
                    }

                    if (ForceApplied && _isGrounded)
                    {
                        _stateMachine.ChangeState(_stateMachine.IdleState);
                        return false;
                    }

                    return true;
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                    if (!ForceApplied)
                    {
                        _player.SetRigidbodyVelocityY(0f);
                        _player.SetMovementByImpulse(Vector2.up, _playerData.JumpForce);
                        ForceApplied = true;
                    }
                }

                protected override void PhysicsFirstStep()
                {
                    base.PhysicsFirstStep();

                    _player.SetGravityScale(_playerData.JumpGravity);
                    _player.SetDrag(_playerData.JumpDrag);
                }

                public override string ToString()
                {
                    return "IdleState";
                }
            }
        }
    }
}



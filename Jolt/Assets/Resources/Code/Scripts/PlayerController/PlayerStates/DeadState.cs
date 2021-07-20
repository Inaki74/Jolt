using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class DeadState : PlayerState
            {
                private float _currentTime;

                // Instantiate particles
                // Move player to last checkpoint (but here we will have only one checkpoint, so skip)
                // Reset objects (but here they are immutable so skip)

                public DeadState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
                {
                }

                public override void DoChecks()
                {
                    base.DoChecks();
                }

                public override void Enter()
                {
                    base.Enter();

                    _player.SetGravityScale(0f);
                    _player.SetMovementX(0f); _player.SetMovementY(0f);
                    _player.SetActivePhysicsCollider(false);
                    _player.InstantiateDeathParticles();
                }

                public override void Exit()
                {
                    base.Exit();

                    _player.ResetPosition();
                    _player.SetActivePhysicsCollider(true);
                    _player.SetGravityScale(1f);
                }

                public override void LogicUpdate()
                {
                    base.LogicUpdate();

                    _currentTime = Time.time;

                    if (_currentTime - _enterTime > _playerData.deadTimer)
                    {
                        _stateMachine.ChangeState(_stateMachine.IdleState);
                    }
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                }

                public override string ToString()
                {
                    return "DeadState";
                }
            }

        }
    }
}
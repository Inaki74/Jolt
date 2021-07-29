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
                protected override Color AssociatedColor => Color.clear;

                private float _currentTime;

                // Instantiate particles
                // Move player to last checkpoint (but here we will have only one checkpoint, so skip)
                // Reset objects (but here they are immutable so skip)

                public DeadState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _player.SetGravityScale(0f);
                    _player.SetRigidbodyVelocityX(0f);
                    _player.SetRigidbodyVelocityY(0f);
                    _player.SetActivePhysicsCollider(false);
                    _player.InstantiateDeathParticles();
                }

                public override void Exit()
                {
                    base.Exit();

                    _player.IsDead = false;
                    _player.ResetPosition();
                    _player.SetActivePhysicsCollider(true);
                    _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                }

                public override bool LogicUpdate()
                {
                    bool continueExecution = base.LogicUpdate();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _currentTime = Time.time;
                    bool hasRespawned = _currentTime - _enterTime > _playerData.DeadTimer;

                    if (hasRespawned)
                    {
                        _stateMachine.ChangeState(_stateMachine.IdleState);
                        return false;
                    }

                    return true;
                }

                public override string ToString()
                {
                    return "DeadState";
                }
            }

        }
    }
}
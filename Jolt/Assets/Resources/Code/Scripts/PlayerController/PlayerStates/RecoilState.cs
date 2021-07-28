using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class RecoilState : GroundedState
            {
                protected override Color AssociatedColor => Color.magenta;

                private float _timeToChange = 0.2f;
                private float _currentTime;

                public RecoilState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override bool LogicUpdate()
                {
                    bool continueExecution = base.LogicUpdate();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _currentTime = Time.time;

                    // After enough time elapses, change to the appropiate state
                    if (_currentTime - _enterTime > _timeToChange)
                    {
                        // Movement -> move
                        if (_isMoving)
                        {
                            _stateMachine.ChangeState(_stateMachine.MoveState);
                            return false;
                        }
                        // No Movement -> idle
                        else
                        {
                            _stateMachine.ChangeState(_stateMachine.IdleState);
                            return false;
                        }
                    }

                    return true;
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                    if (_isMoving)
                    {
                        _player.SetRigidbodyVelocityX(_playerData.MovementSpeed * _moveInput.x);
                    }
                    else
                    {
                        _player.SetRigidbodyVelocityX(0f);
                    }
                }

                public override string ToString()
                {
                    return "RecoilState";
                }
            }
        }
    }
}



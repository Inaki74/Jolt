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
                private float _timeToChange = 0.2f;
                private float _currentTime;

                public RecoilState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
                {
                }

                public override void DoChecks()
                {
                    base.DoChecks();
                }

                public override void Enter()
                {
                    base.Enter();
                }

                public override void Exit()
                {
                    base.Exit();
                }

                public override void LogicUpdate()
                {
                    base.LogicUpdate();

                    _currentTime = Time.time;

                    // After enough time elapses, change to the appropiate state
                    if (_currentTime - _enterTime > _timeToChange)
                    {
                        // Movement -> move
                        if (_moveInput.x != 0)
                        {
                            _stateMachine.ChangeState(_stateMachine.MoveState);
                        }
                        // No Movement -> idle
                        else
                        {
                            _stateMachine.ChangeState(_stateMachine.IdleState);
                        }
                    }
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                    if (_isMoving)
                    {
                        _player.SetMovementX(_playerData.movementSpeed * _moveInput.x);
                    }
                    else
                    {
                        _player.SetMovementX(0f);
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



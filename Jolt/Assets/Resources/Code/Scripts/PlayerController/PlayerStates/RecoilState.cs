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

                private float _currentTime;

                public RecoilState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _currentTime = Time.time;
                    bool recoiledEnoughTime = _currentTime - _enterTime > _playerData.RecoilTimer;

                    // After enough time elapses, change to the appropiate state
                    if (recoiledEnoughTime)
                    {
                        if (_isMoving)
                        {
                            _stateMachine.ScheduleStateChange(_stateMachine.MoveState);
                            return false;
                        }
                        else
                        {
                            _stateMachine.ScheduleStateChange(_stateMachine.IdleState);
                            return false;
                        }
                    }

                    return true;
                }

                protected override void PlayerControlAction()
                {
                    base.PlayerControlAction();
                    //_player.SetRigidbodyVelocityX(_playerData.MovementSpeed * _moveInput.x);
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                }
            }
        }
    }
}



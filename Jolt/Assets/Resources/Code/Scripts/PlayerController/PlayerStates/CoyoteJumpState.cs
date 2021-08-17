using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class CoyoteJumpState : FullControlState
            {
                protected override Color AssociatedColor => Color.magenta;
                protected override string AnimString => PlayerAnimations.Constants.FALL_BOOL;

                private float _currentTime;

                public CoyoteJumpState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                }

                public override void Exit()
                {
                    base.Exit();
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _currentTime = Time.time;

                    bool timeout = _currentTime - _enterTime > _playerData.JumpCoyoteTiming;

                    if (_jumpPressed)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.JumpState);
                        return false;
                    }

                    if (timeout)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.AirborneState);
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
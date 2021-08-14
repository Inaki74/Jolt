using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class MoveState : GroundedState
            {
                protected override Color AssociatedColor => Color.white;

                public MoveState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    _player.SetAnimationBool(PlayerAnimations.Constants.MOVE_BOOL, true);
                }

                public override void Exit()
                {
                    base.Exit();
                    _player.SetAnimationBool(PlayerAnimations.Constants.MOVE_BOOL, false);
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    // No movement -> idle state
                    if (!_isMoving)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.IdleState);
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



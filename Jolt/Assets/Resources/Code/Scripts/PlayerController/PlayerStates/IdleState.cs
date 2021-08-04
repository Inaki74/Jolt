using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class IdleState : GroundedState
            {
                protected override Color AssociatedColor => Color.yellow;

                public IdleState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                }

                public override bool LogicUpdate()
                {
                    bool continueExecution = base.LogicUpdate();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    // Theres movement -> MoveState
                    if (_moveInput.x != 0)
                    {
                        _stateMachine.ChangeState(_stateMachine.MoveState);
                        return false;
                    }

                    return true;
                }

                protected override void PhysicsFirstStep()
                {
                    base.PhysicsFirstStep();
                    _player.SetRigidbodyVelocityX(0f);
                }
            }
        }
    }
}


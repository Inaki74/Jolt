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

                public IdleState(IPlayerStateMachine stateMachine, IPlayer player, PlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _player.SetRigidbodyVelocityX(0f);
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

                public override string ToString()
                {
                    return "IdleState";
                }
            }
        }
    }
}


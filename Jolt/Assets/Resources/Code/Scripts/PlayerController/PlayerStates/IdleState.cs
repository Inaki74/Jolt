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
                public IdleState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _player.SetMovementX(0f);
                }

                public override void LogicUpdate()
                {
                    base.LogicUpdate();

                    // Theres movement -> MoveState
                    if (_moveInput.x != 0)
                    {
                        _stateMachine.ChangeState(_stateMachine.MoveState);
                    }
                }

                public override string ToString()
                {
                    return "IdleState";
                }
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class AliveState : PlayerState
            {
                private bool _isAlive;

                public AliveState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _isAlive = true;
                }

                public override void LogicUpdate()
                {
                    base.LogicUpdate();

                    _isAlive = !_player.CheckIfDead();

                    if (!_isAlive)
                    {
                        _stateMachine.ChangeState(_stateMachine.DeadState);
                    }
                }
            }
        }
    }
}


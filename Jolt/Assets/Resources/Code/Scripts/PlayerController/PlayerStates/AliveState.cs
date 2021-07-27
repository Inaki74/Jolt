using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public abstract class AliveState : PlayerState
            {
                private bool _isAlive;

                public AliveState(IPlayerStateMachine stateMachine, IPlayer player, PlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _isAlive = true;
                }

                public override bool LogicUpdate()
                {
                    base.LogicUpdate();

                    _isAlive = !_player.IsDead;

                    if (!_isAlive)
                    {
                        _stateMachine.ChangeState(_stateMachine.DeadState);
                        return false;
                    }

                    return true;
                }
            }
        }
    }
}


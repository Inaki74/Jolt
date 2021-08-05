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

                public AliveState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _isAlive = true;
                }

                protected override bool StateChangeCheck()
                {
                    base.StateChangeCheck();

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


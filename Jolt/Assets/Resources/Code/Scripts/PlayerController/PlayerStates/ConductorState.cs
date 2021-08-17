using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public abstract class ConductorState : AliveState
            {

                public ConductorState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    
                    _stateMachine.DashingState.ResetAmountOfDashes();
                    _player.SetGravityScale(0f);
                }

                public override void Exit()
                {
                    base.Exit();
                    _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                }
            }
        }
    }
}



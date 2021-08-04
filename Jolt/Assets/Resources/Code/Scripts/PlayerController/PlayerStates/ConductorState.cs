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
                    
                    _stateMachine.PreDashState.ResetAmountOfDashes();
                }

                public override void Exit()
                {
                    base.Exit();
                    _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                }

                protected override void PhysicsFirstStep()
                {
                    base.PhysicsFirstStep();

                    _player.SetGravityScale(0f);
                }
            }
        }
    }
}



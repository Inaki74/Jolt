using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class WallJumpState : AliveState
            {
                protected override Color AssociatedColor => Color.green;

                public Vector2 WallSide { private get; set; }

                public WallJumpState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    //_player.SetRigidbodyVelocityX(0f);
                }

                public override void Exit()
                {
                    base.Exit();

                    WallSide = Vector2.zero;
                }

                public override bool LogicUpdate()
                {
                    bool continueExecution = base.LogicUpdate();

                    if (!continueExecution)
                    {
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



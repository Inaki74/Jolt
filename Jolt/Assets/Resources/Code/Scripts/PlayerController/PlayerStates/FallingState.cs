using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class FallingState : PlayerState
            {
                protected override Color AssociatedColor => Color.green;

                private bool _forceApplied;
                private bool _jumpPressed;
                private bool _isGrounded;

                public FallingState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
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


                    return true;
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();

                }

                public override string ToString()
                {
                    return "IdleState";
                }
            }
        }
    }
}
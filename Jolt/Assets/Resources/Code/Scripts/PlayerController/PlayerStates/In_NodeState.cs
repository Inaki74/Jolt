using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class In_NodeState : ConductorState
            {
                protected override Color AssociatedColor => Color.clear;
                private bool _isStartingDash;

                public In_NodeState(IPlayerStateMachine stateMachine, IPlayer player, PlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Exit()
                {
                    //base.Exit();
                }

                public override bool LogicUpdate()
                {
                    bool continueExecution = base.LogicUpdate();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _isStartingDash = _player.InputManager.DashBegin;

                    if (_isStartingDash)
                    {
                        _stateMachine.ChangeState(_stateMachine.ExitNodeState);
                        return false;
                    }

                    return true;
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();

                    _player.SetPosition(_player.GetNodeInfo().transform.position);
                }

                public override string ToString()
                {
                    return "In_NodeState";
                }
            }
        }
    }
}



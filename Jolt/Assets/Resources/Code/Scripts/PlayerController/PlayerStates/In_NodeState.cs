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
                private bool _isStartingDash;

                public In_NodeState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
                {
                }

                public override void DoChecks()
                {
                    base.DoChecks();
                }

                public override void Enter()
                {
                    base.Enter();
                }

                public override void Exit()
                {
                    //base.Exit();
                }

                public override void LogicUpdate()
                {
                    base.LogicUpdate();

                    _isStartingDash = _player.InputManager.DashBegin;

                    if (_isStartingDash)
                    {
                        _stateMachine.ChangeState(_stateMachine.ExitNodeState);
                    }
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



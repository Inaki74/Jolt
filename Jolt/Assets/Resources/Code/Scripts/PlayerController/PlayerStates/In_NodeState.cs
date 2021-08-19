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
                private Vector2 _moveInput;

                public In_NodeState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _stateMachine.DashingState.LastNode = _player.GetNodeInfo().GetComponent<Node>();

                    _player.Sr.color = AssociatedColor;
                }

                public override void Exit()
                {
                    //base.Exit();

                    _player.Sr.color = Color.white;
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _isStartingDash = _player.InputManager.DashBegin;
                    _moveInput = _player.InputManager.MovementVector;

                    if (_isStartingDash && _moveInput != Vector2.zero)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.ExitNodeState);
                        return false;
                    }

                    return true;
                }

                protected override void PlayerControlAction()
                {
                    base.PlayerControlAction();

                    _player.SetPosition(_player.GetNodeInfo().transform.position);
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                }

                public override string ToString()
                {
                    return "In_NodeState";
                }
            }
        }
    }
}



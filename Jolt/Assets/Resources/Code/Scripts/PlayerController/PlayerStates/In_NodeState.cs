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
                private const float MAX_DASH_TIMEOUT = 0.1f;

                protected override Color AssociatedColor => Color.clear;
                private bool _isStartingDash;
                private Vector3 _finalDashInput;

                private float _dashPressedTimeout =MAX_DASH_TIMEOUT;

                public In_NodeState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _stateMachine.DashingState.LastNode = _player.GetNodeInfo().GetComponent<Node>();

                    _player.Sr.color = AssociatedColor;
                    _dashPressedTimeout = MAX_DASH_TIMEOUT;
                }

                public override void Exit()
                {
                    //base.Exit();

                    _player.Sr.color = Color.white;
                    _dashPressedTimeout = MAX_DASH_TIMEOUT;
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _isStartingDash = _player.InputManager.DashBegin;
                    _finalDashInput = _player.InputManager.FinalDashPoint;
                    bool startedDashRecently = _dashPressedTimeout < MAX_DASH_TIMEOUT;

                    if ((_isStartingDash || startedDashRecently) && _finalDashInput != Vector3.zero)
                    {
                        Debug.Log(_finalDashInput);
                        _stateMachine.ScheduleStateChange(_stateMachine.ExitNodeState);
                        return false;
                    }

                    if (_isStartingDash)
                    {
                        _dashPressedTimeout = 0f;
                    }

                    if(startedDashRecently)
                    {
                        _dashPressedTimeout += Time.deltaTime;
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



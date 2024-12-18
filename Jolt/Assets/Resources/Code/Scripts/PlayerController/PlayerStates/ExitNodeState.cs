﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class ExitNodeState : ConductorState
            {
                protected override Color AssociatedColor => Color.magenta;
                // Known bug: If two nodes are too close to each other, doesnt work appropiately

                private bool _isDashStarted;
                private float _currentTime;

                public ExitNodeState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _enterTime = Time.time;
                    _isDashStarted = true;
                }

                public override void Exit()
                {
                    base.Exit();

                    //_stateMachine.DashingState.LastNode = new Node();
                    _stateMachine.DashingState.ResetAmountOfDashes();
                    _stateMachine.DashingState.WasInNode = true;
                    //_stateMachine.DashingState.DecreaseAmountOfDashes();
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _currentTime = Time.time;
                    _isDashStarted = _player.InputManager.DashBegin && (_currentTime - _enterTime < 0);

                    //Cant be Cancelled, go to dashing when stopped pressing or after timeout
                    if (!_isDashStarted)
                    {
                        float moveInputX = _player.InputManager.MovementVector.x;
                        _player.CheckIfShouldFlip(moveInputX);

                        _stateMachine.ScheduleStateChange(_stateMachine.DashingState);
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
                    return "ExitNodeState";
                }
            }
        }
    }
}
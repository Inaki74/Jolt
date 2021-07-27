﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class ExitRailState : AliveState
            {
                protected override Color AssociatedColor => Color.magenta;

                public Vector2 ExitVector { private get; set; }

                public float ExitSpeed { private get; set; }

                private Vector2 _moveInput;
                private bool _isGrounded;
                private bool _isStartingDash;

                public ExitRailState(IPlayerStateMachine stateMachine, IPlayer player, PlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    ExitVector.Normalize();
                    _player.SetMovementByImpulse(ExitVector, ExitSpeed);
                }

                public override bool LogicUpdate()
                {
                    bool continueExecution = base.LogicUpdate();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _isGrounded = _player.CheckIsGrounded();
                    _moveInput = _player.InputManager.MovementVector;
                    _isStartingDash = _player.InputManager.DashBegin;

                    if (_isGrounded)
                    {
                        _stateMachine.ChangeState(_stateMachine.RecoilState);
                        return false;
                    }
                    else if (Mathf.Abs(_player.GetCurrentVelocity().x) < 0.2f)
                    {
                        _stateMachine.ChangeState(_stateMachine.AirborneState);
                        return false;
                    }
                    else if (_isStartingDash && _stateMachine.PreDashState.CanDash())
                    {
                        _stateMachine.ChangeState(_stateMachine.PreDashState);
                        return false;
                    }

                    return true;
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();

                    //Exit right
                    if (ExitVector.x > 0)
                    {
                        if (_player.GetCurrentVelocity().x < _playerData.movementSpeed || _moveInput.x < 0)
                        {
                            _player.SetMovementXByForce(_moveInput, _playerData.movementSpeed + ExitSpeed);
                        }
                    }
                    //Exit left
                    else
                    {
                        if (_player.GetCurrentVelocity().x > -_playerData.movementSpeed || _moveInput.x > 0)
                        {
                            _player.SetMovementXByForce(_moveInput, _playerData.movementSpeed + ExitSpeed);
                        }
                    }
                }

                public override string ToString()
                {
                    return "ExitRailState";
                }
            }
        }
    }
}



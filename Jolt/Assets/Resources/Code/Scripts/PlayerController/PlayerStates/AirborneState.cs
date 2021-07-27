﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class AirborneState : AliveState
            {
                protected override Color AssociatedColor => Color.red;

                private Vector2 _moveInput;
                private bool _isGrounded;
                private bool _isStartingDash;
                private bool _isMoving;

                public AirborneState(IPlayerStateMachine stateMachine, IPlayer player, PlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void DoChecks()
                {
                    base.DoChecks();

                    _isMoving = _moveInput.x != 0;
                }

                public override bool LogicUpdate()
                {
                    bool continueExecution = base.LogicUpdate();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _moveInput = _player.InputManager.MovementVector;
                    _isGrounded = _player.CheckIsGrounded();
                    _isStartingDash = _player.InputManager.DashBegin;

                    // If it hits ground -> recoil
                    if (_isGrounded)
                    {
                        _stateMachine.ChangeState(_stateMachine.RecoilState);
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

                    if (_isMoving)
                    {
                        if (_stateMachine.LastState == "ExitRailState")
                        {
                            _player.SetMovementXByForce(Vector2.right, _playerData.movementSpeed * _moveInput.x);
                        }
                        else
                        {
                            _player.SetRigidbodyVelocityX(_playerData.movementSpeed * _moveInput.x);
                        }
                    }
                    else
                    {
                        if (_stateMachine.LastState != "ExitRailState")
                        {
                            _player.SetRigidbodyVelocityX(0f);
                        }
                    }
                }

                public override string ToString()
                {
                    return "AirborneState";
                }
            }
        }
    }
}



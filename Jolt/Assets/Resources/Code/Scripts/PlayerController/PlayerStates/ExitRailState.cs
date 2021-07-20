using System.Collections;
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
                public Vector2 ExitVector { private get; set; }

                public float ExitSpeed { private get; set; }

                private Vector2 _moveInput;
                private bool _isGrounded;
                private bool _isStartingDash;

                public ExitRailState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
                {
                }

                public override void DoChecks()
                {
                    base.DoChecks();
                }

                public override void Enter()
                {
                    base.Enter();

                    ExitVector.Normalize();
                    _player.SetForceToGivenVector(ExitVector, ExitSpeed);
                }

                public override void Exit()
                {
                    base.Exit();
                }

                public override void LogicUpdate()
                {
                    base.LogicUpdate();

                    _isGrounded = _player.CheckIsGrounded();
                    _moveInput = _player.InputManager.MovementVector;
                    _isStartingDash = _player.InputManager.DashBegin;

                    if (_isGrounded)
                    {
                        _stateMachine.ChangeState(_stateMachine.RecoilState);
                    }
                    else if (Mathf.Abs(_player.GetCurrentVelocity().x) < 0.2f)
                    {
                        _stateMachine.ChangeState(_stateMachine.AirborneState);
                    }
                    else if (_isStartingDash && _stateMachine.PreDashState.CanDash())
                    {
                        _stateMachine.ChangeState(_stateMachine.PreDashState);
                    }
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();

                    //Exit right
                    if (ExitVector.x > 0)
                    {
                        if (_player.GetCurrentVelocity().x < _playerData.movementSpeed || _moveInput.x < 0)
                            _player.SetMovementXByForce(_moveInput, _playerData.movementSpeed + ExitSpeed);
                    }
                    //Exit left
                    else
                    {
                        if (_player.GetCurrentVelocity().x > -_playerData.movementSpeed || _moveInput.x > 0)
                            _player.SetMovementXByForce(_moveInput, _playerData.movementSpeed + ExitSpeed);
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



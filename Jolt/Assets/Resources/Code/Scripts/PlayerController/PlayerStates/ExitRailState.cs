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
                protected override Color AssociatedColor => Color.magenta;

                public Vector2 ExitVector { private get; set; }

                public float ExitSpeed { private get; set; }

                private Vector2 _moveInput;
                private bool _isGrounded;
                private bool _isStartingDash;

                public ExitRailState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    ExitVector.Normalize();
                    //_player.SetMovementByImpulse(ExitVector, ExitSpeed);
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _isGrounded = _player.CheckIsGrounded();
                    _moveInput = _player.InputManager.MovementVector;
                    _isStartingDash = _player.InputManager.DashBegin;

                    if (_isGrounded)
                    {
                        _stateMachine.ChangeState(_stateMachine.IdleState);
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

                protected override void PlayerControlAction()
                {
                    base.PlayerControlAction();
                    //Exit right
                    if (ExitVector.x > 0)
                    {
                        if (_player.GetCurrentVelocity().x < _playerData.MovementSpeed || _moveInput.x < 0)
                        {
                            //_player.SetMovementXByForce(_moveInput, _playerData.MovementSpeed + ExitSpeed);
                        }
                    }
                    //Exit left
                    else
                    {
                        if (_player.GetCurrentVelocity().x > -_playerData.MovementSpeed || _moveInput.x > 0)
                        {
                            //_player.SetMovementXByForce(_moveInput, _playerData.MovementSpeed + ExitSpeed);
                        }
                    }
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                }

                public override string ToString()
                {
                    return "ExitRailState";
                }
            }
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class CoyoteJumpState : PlayerState
            {
                protected override Color AssociatedColor => Color.magenta;

                private Vector2 _moveInput;
                private bool _jumpPressed;
                private float _currentTime;

                public CoyoteJumpState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                }

                public override void Exit()
                {
                    base.Exit();
                }

                public override bool LogicUpdate()
                {
                    bool continueExecution = base.LogicUpdate();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _currentTime = Time.time;

                    _moveInput = _player.InputManager.MovementVector;
                    _jumpPressed = _player.InputManager.JumpPressed;
                    bool timeout = _currentTime - _enterTime > _playerData.CoyoteTiming;

                    if (timeout)
                    {
                        _stateMachine.ChangeState(_stateMachine.AirborneState);
                        return false;
                    }

                    if (_jumpPressed)
                    {
                        _stateMachine.ChangeState(_stateMachine.JumpState);
                        return false;
                    }

                    return true;
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                    _player.SetRigidbodyVelocityX(_playerData.MovementSpeed * _moveInput.x);
                }

                public override string ToString()
                {
                    return "IdleState";
                }
            }
        }
    }
}
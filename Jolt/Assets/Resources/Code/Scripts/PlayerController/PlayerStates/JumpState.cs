using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class JumpState : PlayerState
            {
                protected override Color AssociatedColor => Color.green;

                private bool _forceApplied;
                private bool _jumpPressed;
                private bool _isGrounded;

                public JumpState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _forceApplied = false;
                }

                public override bool LogicUpdate()
                {
                    bool continueExecution = base.LogicUpdate();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _jumpPressed = _player.InputManager.JumpPressed;
                    _isGrounded = _player.CheckIsGrounded();

                    if (_forceApplied)
                    {
                        if (_isGrounded)
                        {
                            _stateMachine.ChangeState(_stateMachine.IdleState);
                            return false;
                        }

                        if (_jumpPressed)
                        {

                        }

                        _stateMachine.ChangeState(_stateMachine.AirborneState);
                        return false;
                    }

                    return true;
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();

                    if (!_forceApplied)
                    {
                        _player.SetMovementByImpulse(Vector2.up, _playerData.JumpForce);
                        _forceApplied = true;
                    }
                }

                public override string ToString()
                {
                    return "IdleState";
                }
            }
        }
    }
}
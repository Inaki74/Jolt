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

                private bool _playOnce;
                private bool _jumpPressed;
                private bool _isGrounded;

                public JumpState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _playOnce = true;
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

                    if (_isGrounded)
                    {
                        _stateMachine.ChangeState(_stateMachine.IdleState);
                        return false;
                    }
                    else
                    {
                        if (_jumpPressed)
                        {

                        }
                        else
                        {
                            _stateMachine.ChangeState(_stateMachine.AirborneState);
                            return false;
                        }
                    }

                    

                    return true;
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();

                    if (_playOnce)
                    {
                        _player.SetMovementByImpulse(Vector2.up, _playerData.JumpForce);
                        _playOnce = false;
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
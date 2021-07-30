using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public abstract class GroundedState : AliveState
            {
                protected Vector2 _moveInput;
                protected bool _isGrounded;
                protected bool _isStartingDash;
                protected bool _isMoving;
                protected bool _jumpPressed;

                public GroundedState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _stateMachine.PreDashState.ResetAmountOfDashes();
                }

                public override bool LogicUpdate()
                {
                    bool continueExecution = base.LogicUpdate();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _moveInput = _player.InputManager.MovementVector;
                    _jumpPressed = _player.InputManager.JumpPressed;
                    _isStartingDash = _player.InputManager.DashBegin;
                    _isGrounded = _player.CheckIsGrounded();
                    _isMoving = _moveInput.x != 0;

                    if (_isStartingDash)
                    {
                        _stateMachine.ChangeState(_stateMachine.PreDashState);
                        return false;
                    }

                    if (!_isGrounded)
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
            }
        }
    }
}



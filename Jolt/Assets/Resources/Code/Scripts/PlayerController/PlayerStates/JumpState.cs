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
                private Vector2 _moveInput;
                private bool _isStartingDash;

                public JumpState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _forceApplied = false;
                    _player.SetGravityScale(_playerData.JumpGravity);
                    _player.SetDrag(_playerData.JumpDrag);
                }

                public override void Exit()
                {
                    base.Exit();

                    _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                    _player.SetDrag(_playerData.PlayerPhysicsData.StandardLinearDrag);
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
                    bool canDash = _stateMachine.PreDashState.CanDash();
                    bool reachedPeak = _player.CheckIsFreeFalling();

                    if (_isStartingDash && canDash)
                    {
                        _stateMachine.ChangeState(_stateMachine.PreDashState);
                        return false;
                    }

                    if (_isGrounded && _forceApplied)
                    {
                        _stateMachine.ChangeState(_stateMachine.IdleState);
                        return false;
                    }

                    if (!_jumpPressed || reachedPeak)
                    {
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
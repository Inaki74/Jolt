using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class FloatingState : PlayerState
            {
                protected override Color AssociatedColor => Color.green;

                private bool _forceApplied;
                private bool _jumpHeld;
                private bool _isGrounded;
                private Vector2 _moveInput;
                private bool _isStartingDash;
                private bool _canDash;
                private bool _reachedPeak;

                public FloatingState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _forceApplied = false;
                    _player.SetGravityScale(_playerData.FloatGravity);
                    _player.SetDrag(_playerData.FloatDrag);
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
                    _jumpHeld = _player.InputManager.JumpHeld;
                    _isStartingDash = _player.InputManager.DashBegin;
                    _canDash = _stateMachine.PreDashState.CanDash();
                    _reachedPeak = _player.CheckIsFreeFalling();

                    if (_isStartingDash && _canDash)
                    {
                        _stateMachine.ChangeState(_stateMachine.PreDashState);
                        return false;
                    }

                    if (!_jumpHeld || _reachedPeak)
                    {
                        _stateMachine.ChangeState(_stateMachine.AirborneState);
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
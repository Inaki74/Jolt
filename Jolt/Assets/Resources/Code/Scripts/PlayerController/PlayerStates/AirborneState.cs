using System.Collections;
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
                private Vector2 _moveInput;
                private bool _isGrounded;
                private bool _isStartingDash;
                private bool _isMoving;

                public AirborneState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
                {
                }

                public override void DoChecks()
                {
                    base.DoChecks();
                    _isMoving = _moveInput.x != 0;
                }

                public override void Enter()
                {
                    base.Enter();
                }

                public override void Exit()
                {
                    base.Exit();
                }

                public override void LogicUpdate()
                {
                    base.LogicUpdate();

                    _moveInput = _player.InputManager.MovementVector;
                    _isGrounded = _player.CheckIsGrounded();
                    _isStartingDash = _player.InputManager.DashBegin;

                    // If it hits ground -> recoil
                    if (_isGrounded)
                    {
                        _stateMachine.ChangeState(_stateMachine.RecoilState);
                    }
                    else if (_isStartingDash && _stateMachine.PreDashState.CanDash())
                    {
                        _stateMachine.ChangeState(_stateMachine.PreDashState);
                    }
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                    if (_isMoving)
                    {
                        if (_stateMachine.LastState == "ExitRailState")
                            _player.SetMovementXByForce(Vector2.right, _playerData.movementSpeed * _moveInput.x);
                        else
                            _player.SetMovementX(_playerData.movementSpeed * _moveInput.x);
                    }
                    else
                    {
                        if (_stateMachine.LastState != "ExitRailState")
                            _player.SetMovementX(0f);
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



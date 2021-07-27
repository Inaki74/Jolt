using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class DashingState : AliveState
            {
                protected override Color AssociatedColor => Color.cyan;

                private bool _isGrounded;
                private Vector2 _moveInput;
                private float _currentTime;
                private bool _isTouchingNode;
                private bool _isTouchingRail;

                private bool _playOnce;

                public DashingState(IPlayerStateMachine stateMachine, IPlayer player, PlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _playOnce = true;
                    _player.SetGravityScale(0f);
                }

                public override void Exit()
                {
                    base.Exit();

                    _player.SetGravityScale(1f);
                    _player.SetRigidbodyVelocityX(0f);
                    _player.SetRigidbodyVelocityY(0f);
                }

                public override bool LogicUpdate()
                {
                    bool continueExecution = base.LogicUpdate();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _isGrounded = _player.CheckIsGrounded();
                    _moveInput = _player.InputManager.MovementVector;
                    _isTouchingNode = _player.CheckIsTouchingNode();
                    _isTouchingRail = _player.CheckIsTouchingRail();

                    if (_isTouchingNode)
                    {
                        _stateMachine.ChangeState(_stateMachine.InNodeState);
                        return false;
                    }
                    else if (_isTouchingRail)
                    {
                        _stateMachine.ChangeState(_stateMachine.InRailState);
                        return false;
                    }

                    if (_currentTime - _enterTime > _playerData.dashTimeOut)
                    {
                        if (_isGrounded)
                        {
                            if (_moveInput.x != 0)
                            {
                                _stateMachine.ChangeState(_stateMachine.MoveState);
                                return false;
                            }
                            else
                            {
                                _stateMachine.ChangeState(_stateMachine.IdleState);
                                return false;
                            }
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
                    _currentTime = Time.time;

                    if (_playOnce)
                    {
                        _player.SetDashMovement(_playerData.dashSpeed);

                        _playOnce = false;
                    }
                }

                public override string ToString()
                {
                    return "DashingState";
                }
            }
        }
    }
}

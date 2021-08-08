﻿using System.Collections;
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

                public Node LastNode { private get; set; } = new Node();

                private bool _isNotLastNode;
                private bool _wasInNode;
                private bool _isGrounded;
                private Vector2 _moveInput;
                private float _currentTime;
                private bool _isTouchingNode;
                private bool _isTouchingRail;

                private bool _playOnce;

                public DashingState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _playOnce = true;
                    _player.SetGravityScale(0f);
                    _wasInNode = _player.CheckIsTouchingNode();
                    //_player.SetActivePhysicsCollider(false);
                    //_player.SetDashCollider(true);
                }

                public override void Exit()
                {
                    base.Exit();

                    _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                    _player.Velocity = Vector2.zero;
                    //_player.SetActivePhysicsCollider(true);
                    //_player.SetDashCollider(false);
                    //_player.SetRigidbodyVelocityX(0f);
                    //_player.SetRigidbodyVelocityY(0f);
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
                    _isTouchingNode = _player.CheckIsTouchingNode();
                    _isTouchingRail = _player.CheckIsTouchingRail();


                    if (CheckIsAdmittableToGetIntoNode())
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.InNodeState);
                        return false;
                    }

                    if (_isTouchingRail)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.InRailState);
                        return false;
                    }

                    if (_currentTime - _enterTime > _playerData.DashTimeOut)
                    {
                        if (_isGrounded)
                        {
                            if (_moveInput.x != 0)
                            {
                                _stateMachine.ScheduleStateChange(_stateMachine.MoveState);
                                return false;
                            }
                            else
                            {
                                _stateMachine.ScheduleStateChange(_stateMachine.IdleState);
                                return false;
                            }
                        }
                        else
                        {
                            _stateMachine.ScheduleStateChange(_stateMachine.AirborneState);
                            return false;
                        }
                    }

                    return true;
                }

                protected override void PlayerControlAction()
                {
                    base.PlayerControlAction();

                    _currentTime = Time.time;

                    if (_playOnce)
                    {
                        _player.Dash(_playerData.DashSpeed);

                        _playOnce = false;
                    }
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                }

                public override string ToString()
                {
                    return "DashingState";
                }

                public void ResetLastnode()
                {
                    LastNode = new Node();
                }

                private bool CheckIsAdmittableToGetIntoNode()
                {
                    _isNotLastNode = false;
                    Collider2D lastNodeCollider = _player.GetNodeInfo();

                    if (lastNodeCollider != null)
                    {
                        Node currentNode = _player.GetNodeInfo().GetComponent<Node>();

                        _isNotLastNode = currentNode.GetInstanceID() != LastNode.GetInstanceID();
                    }

                    if (_wasInNode)
                    {
                        if (!_isTouchingNode)
                        {
                            _stateMachine.DashingState.ResetLastnode();
                            _wasInNode = false;
                        }
                    }

                    return _isTouchingNode && _isNotLastNode && !_wasInNode;
                }
            }
        }
    }
}

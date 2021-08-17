using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class DashingState : AliveState, ICanDash
            {
                protected override Color AssociatedColor => Color.cyan;
                protected override string AnimString => PlayerAnimations.Constants.DASH_BOOL;

                public Node LastNode { private get; set; } = null;

                private bool _isNotLastNode;
                private bool _wasInNode;
                private bool _isGrounded;
                private Vector2 _moveInput;
                private float _currentTime;
                private bool _isTouchingNode;
                private bool _isTouchingRail;

                private bool _flippedY;
                private int _amountOfDashes;

                private bool _playOnce;

                public DashingState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                    ResetAmountOfDashes();
                }

                public override void Enter()
                {
                    base.Enter();


                    _moveInput = _player.InputManager.MovementVector;
                    _playOnce = true;
                    _player.SetGravityScale(0f);
                    _wasInNode = _player.CheckIsTouchingNode();
                    DecreaseAmountOfDashes();

                    _player.SetAnimationInt(PlayerAnimations.Constants.DASHX_INT, (int)_moveInput.x);
                    _player.SetAnimationInt(PlayerAnimations.Constants.DASHY_INT, (int)_moveInput.y);

                    if(_moveInput.y < 0)
                    {
                        _flippedY = true;
                        _player.FlipY();
                    }
                    //_player.SetActivePhysicsCollider(false);
                    //_player.SetDashCollider(true);
                }

                public override void Exit()
                {
                    base.Exit();

                    _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                    _player.Velocity = Vector2.zero;

                    _player.SetAnimationInt(PlayerAnimations.Constants.DASHX_INT, 0);
                    _player.SetAnimationInt(PlayerAnimations.Constants.DASHY_INT, 0);

                    if (_flippedY)
                    {
                        _flippedY = false;
                        _player.FlipY();
                    }
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
                    //_moveInput = _player.InputManager.MovementVector;

                    _isTouchingNode = _player.CheckIsTouchingNode();
                    _isTouchingRail = _player.CheckIsTouchingRail();
                    bool isTouchingWall = _player.CheckIsTouchingWallLeft() || _player.CheckIsTouchingWallRight();

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

                    if (isTouchingWall && _moveInput.x != 0f)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.WallSlideState);
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
                        _player.SetDashVectors(_player.InputManager.InitialDashPoint, _player.InputManager.FinalDashPoint);
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
                    LastNode = null;
                }

                public bool CanDash()
                {
                    return _amountOfDashes > 0;
                }

                public void ResetAmountOfDashes()
                {
                    _amountOfDashes = _playerData.AmountOfDashes;
                }

                public void DecreaseAmountOfDashes()
                {
                    _amountOfDashes--;
                }

                private bool CheckIsAdmittableToGetIntoNode()
                {
                    _isNotLastNode = false;
                    Collider2D lastNodeCollider = _player.GetNodeInfo();

                    if (lastNodeCollider != null && LastNode != null)
                    {
                        Node currentNode = _player.GetNodeInfo().GetComponent<Node>();

                        _isNotLastNode = currentNode.GetInstanceID() != LastNode.GetInstanceID();
                    }

                    if (LastNode == null)
                    {
                        _isNotLastNode = true;
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

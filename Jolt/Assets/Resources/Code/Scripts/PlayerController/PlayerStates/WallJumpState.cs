using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class WallJumpState : FullControlState
            {
                protected override Color AssociatedColor => Color.green;

                private bool _forceApplied;
                private float _currentTime;

                public Vector2 JumpDirection { private get; set; }

                public WallJumpState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _forceApplied = false;
                    _canMove = false;
                    _player.SetGravityScale(_playerData.WallJumpGravity);
                    _player.SetDrag(_playerData.WallJumpDrag);
                }

                public override void Exit()
                {
                    base.Exit();

                    JumpDirection = Vector2.zero;
                    _canMove = true;
                    _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                    _player.SetDrag(_playerData.PlayerPhysicsData.StandardLinearDrag);
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _currentTime = Time.time;

                    bool timeout = _currentTime - _enterTime > _playerData.WallJumpDuration;
                    _jumpHeld = _player.InputManager.JumpHeld;

                    if (_forceApplied)
                    {
                        if (timeout)
                        {
                            if (_jumpHeld)
                            {
                                _stateMachine.ScheduleStateChange(_stateMachine.FloatingState);
                                return false;
                            }

                            _stateMachine.ScheduleStateChange(_stateMachine.AirborneState);
                            return false;
                        }
                    }

                    return true;
                }

                protected override void PlayerControlAction()
                {
                    base.PlayerControlAction();

                    if (!_forceApplied)
                    {
                        //_player.SetRigidbodyVelocityY(0f);
                        _player.Velocity = new Vector2(_player.Velocity.x, 0f);
                        WallJump();
                        _forceApplied = true;
                    }
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                }

                private void WallJump()
                {
                    float speed = _playerData.WallJumpForceHorizontal;
                    float horizontalForce = 1;
                    float verticalForce = horizontalForce * _playerData.WallJumpForceVerticalRatioWithHorizontal;

                    Vector2 impulseDirection = new Vector2(horizontalForce, verticalForce);
                    Vector2 side = new Vector2(JumpDirection.x, Mathf.Abs(JumpDirection.x));
                    impulseDirection = impulseDirection * side * speed;

                    _player.Velocity = impulseDirection;

                    //_player.MoveX(impulseDirection.x);
                    //_player.MoveY(impulseDirection.y);

                    //_player.SetMovementByImpulse(impulseDirection, speed);
                }
            }
        }
    }
}



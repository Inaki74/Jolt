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
                }

                public override void Exit()
                {
                    base.Exit();

                    JumpDirection = Vector2.zero;
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

                    _currentTime = Time.time;

                    bool timeout = _currentTime - _enterTime > _playerData.WallJumpDuration; // TODO: Make this a variable or maybe i dont need it.
                    _jumpHeld = _player.InputManager.JumpHeld;

                    if (_forceApplied)
                    {
                        if (timeout)
                        {
                            if (_jumpHeld)
                            {
                                _stateMachine.ChangeState(_stateMachine.FloatingState);
                                return false;
                            }

                            _stateMachine.ChangeState(_stateMachine.AirborneState);
                            return false;
                        }
                    }

                    return true;
                }

                public override void PhysicsUpdate()
                {
                    //base.PhysicsUpdate();

                    if (!_forceApplied)
                    {
                        _player.SetRigidbodyVelocityY(0f);
                        WallJump();
                        _forceApplied = true;
                    }
                }

                protected override void PhysicsFirstStep()
                {
                    base.PhysicsFirstStep();

                    _player.SetGravityScale(_playerData.WallJumpGravity);
                    _player.SetDrag(_playerData.WallJumpDrag);
                }

                private void WallJump()
                {
                    float speed = _playerData.WallJumpForceHorizontal;
                    float horizontalForce = 1;
                    float verticalForce = horizontalForce * _playerData.WallJumpForceVerticalRatioWithHorizontal;

                    Vector2 impulseDirection = new Vector2(horizontalForce, verticalForce);
                    Vector2 side = new Vector2(JumpDirection.x, Mathf.Abs(JumpDirection.x));
                    impulseDirection = impulseDirection * side;

                    _player.SetMovementByImpulse(impulseDirection, speed);
                }
            }
        }
    }
}



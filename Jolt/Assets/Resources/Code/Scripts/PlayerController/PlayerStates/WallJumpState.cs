using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class WallJumpState : AliveState
            {
                protected override Color AssociatedColor => Color.green;

                private bool _jumpInput;
                private bool _forceApplied;
                private bool _isTouchingWall;
                private float _currentTime;

                public Vector2 WallSide { private get; set; }

                public WallJumpState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _forceApplied = false;
                    //_player.SetRigidbodyVelocityX(0f);
                }

                public override void Exit()
                {
                    base.Exit();

                    WallSide = Vector2.zero;
                }

                public override bool LogicUpdate()
                {
                    bool continueExecution = base.LogicUpdate();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _currentTime = Time.time;

                    //_isTouchingWall = _player.CheckIsTouchingWallLeft() || _player.CheckIsTouchingWallRight();
                    bool a = _currentTime - _enterTime > 0.2f;

                    if (_forceApplied && a)
                    {
                        _stateMachine.ChangeState(_stateMachine.AirborneState);
                    }

                    return true;
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();

                    if (!_forceApplied)
                    {
                        WallJump();
                        _forceApplied = true;
                    }
                }

                public override string ToString()
                {
                    return "IdleState";
                }

                private void WallJump()
                {
                    float speed = _playerData.WallJumpForceHorizontal;
                    float horizontalForce = 1;
                    float verticalForce = horizontalForce * _playerData.WallJumpForceVerticalRatioWithHorizontal;

                    Vector2 impulseDirection = new Vector2(horizontalForce, verticalForce);
                    Vector2 side = new Vector2(-WallSide.x, 1);
                    impulseDirection = impulseDirection * side;

                    _player.SetMovementByImpulse(impulseDirection, speed);
                }
            }
        }
    }
}



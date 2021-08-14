using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class WallSlideFloatingState : OnWallState
            {
                protected override Color AssociatedColor => Color.gray;

                private bool _reachedPeak;
                private float _gravitySet = 0f;

                public WallSlideFloatingState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    if (_gravitySet != 0f)
                    {
                        _player.SetGravityScale(_gravitySet);
                    }
                    else
                    {
                        _player.SetGravityScale(_playerData.FloatGravity);
                    }
                }

                public override void Exit()
                {
                    base.Exit();

                    _gravitySet = 0f;
                    _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                }

                protected override bool StateChangeCheck()
                {
                    if (!CheckFloatingStateChange())
                    {
                        return false;
                    }

                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _reachedPeak = _player.CheckIsFreeFalling();
                    bool isMovingRight = _moveInput.x > 0f;
                    bool isMovingLeft = _moveInput.x < 0f;

                    if (!_jumpHeld || _reachedPeak)
                    {
                        if((_isTouchingWallLeft && isMovingLeft) ||
                            (_isTouchingWallRight && isMovingRight))
                        {
                            _stateMachine.ScheduleStateChange(_stateMachine.WallSlideState);
                            return false;
                        }

                        _stateMachine.ScheduleStateChange(_stateMachine.WallAirborneState);
                        return false;
                    }

                    return true;
                }

                protected override void PlayerControlAction()
                {
                    base.PlayerControlAction();
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                }

                public void SetGravityScale(float newScale)
                {
                    _gravitySet = newScale;
                }

                private bool CheckFloatingStateChange()
                {
                    _jumpHeld = _player.InputManager.JumpHeld;
                    _isTouchingWallLeft = _player.CheckIsTouchingWallLeft();
                    _isTouchingWallRight = _player.CheckIsTouchingWallRight();

                    bool isTouchingWall = _isTouchingWallLeft || _isTouchingWallRight;

                    if (_jumpHeld && !isTouchingWall)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.FloatingState);
                        return false;
                    }

                    return true;
                }

            }
        }
    }
}



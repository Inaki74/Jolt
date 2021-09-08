using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class DashFloatingState : FullControlState
            {
                private Vector2 _dashFinalPoint;

                private float _currentTime;

                private bool _isTouchingWallLeft;
                private bool _isTouchingWallRight;

                public DashFloatingState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    _dashFinalPoint = _player.InputManager.FinalDashPoint;
                    _player.SetGravityScale(_playerData.DashFloatingGravityScale);
                }

                public override void Exit()
                {
                    base.Exit();

                    _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _currentTime = Time.time;

                    _isTouchingWallLeft = _player.CheckIsTouchingWallLeft();
                    _isTouchingWallRight = _player.CheckIsTouchingWallRight();
                    bool isTouchingWall = _isTouchingWallLeft || _isTouchingWallRight;
                    bool onLeftWallAndMovingTowardsIt = _isTouchingWallLeft && _moveInput.x < 0f;
                    bool onRightWallAndMovingTowardsIt = _isTouchingWallRight && _moveInput.x > 0f;

                    if (isTouchingWall && (onLeftWallAndMovingTowardsIt || onRightWallAndMovingTowardsIt))
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.WallSlideState);
                        return false;
                    }

                    if (_currentTime - _enterTime > _playerData.DashFloatingTimeout)
                    {
                        if (isTouchingWall)
                        {
                            _stateMachine.ScheduleStateChange(_stateMachine.WallAirborneState);
                            return false;
                        }

                        _stateMachine.ScheduleStateChange(_stateMachine.AirborneState);
                        return false;
                    }

                    return true;
                }
            }
        }
    }
}



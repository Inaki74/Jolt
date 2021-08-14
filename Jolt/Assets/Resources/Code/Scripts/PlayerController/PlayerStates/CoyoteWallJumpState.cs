using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class CoyoteWallJumpState : FullControlState
            {
                protected override Color AssociatedColor => Color.magenta;

                public Vector2 WallSide { private get; set; }

                private float _currentTime;
                private bool _isTouchingWallLeft;
                private bool _isTouchingWallRight;

                public CoyoteWallJumpState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                }

                public override void Exit()
                {
                    base.Exit();
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _currentTime = Time.time;

                    bool timeout = _currentTime - _enterTime > _playerData.WallJumpCoyoteTiming;
                    _isTouchingWallLeft = _player.CheckIsTouchingWallLeft();
                    _isTouchingWallRight = _player.CheckIsTouchingWallRight();
                    bool isTouchingWall = _isTouchingWallLeft || _isTouchingWallRight;

                    if (_jumpPressed)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.WallJumpState);
                        return false;
                    }

                    if (timeout)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.AirborneState);
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
            }
        }
    }
}
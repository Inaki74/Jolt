using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class FloatingState : FullControlState
            {
                protected override Color AssociatedColor => Color.green;

                private bool _reachedPeak;
                private bool _isTouchingWall;
                

                public FloatingState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _player.SetGravityScale(_playerData.FloatGravity);
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
                    _reachedPeak = _player.CheckIsFreeFalling();
                    _isTouchingWall = _player.CheckIsTouchingWallLeft() || _player.CheckIsTouchingWallRight();

                    if (!_jumpHeld || _reachedPeak)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.AirborneState);
                        return false;
                    }

                    if (_isTouchingWall)
                    {
                        _stateMachine.WallSlideFloatingState.SetGravityScale(_playerData.FloatingGravityScaleIntoWall);
                        _stateMachine.ScheduleStateChange(_stateMachine.WallSlideFloatingState);
                        return false;
                    }

                    return true;
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                }

                
            }
        }
    }
}
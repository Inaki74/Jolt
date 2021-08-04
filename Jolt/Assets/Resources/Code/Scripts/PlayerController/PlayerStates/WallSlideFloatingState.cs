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

                public WallSlideFloatingState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                }

                public override void Exit()
                {
                    base.Exit();

                    _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                    _player.SetDrag(_playerData.PlayerPhysicsData.StandardLinearDrag);
                }

                public override bool LogicUpdate()
                {
                    _jumpHeld = _player.InputManager.JumpHeld;
                    _isTouchingWallLeft = _player.CheckIsTouchingWallLeft();
                    _isTouchingWallRight = _player.CheckIsTouchingWallRight();

                    bool isTouchingWall = _isTouchingWallLeft || _isTouchingWallRight;

                    if (_jumpHeld && !isTouchingWall)
                    {
                        _stateMachine.ChangeState(_stateMachine.FloatingState);
                        return false;
                    }

                    bool continueExecution = base.LogicUpdate();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _reachedPeak = _player.CheckIsFreeFalling();

                    if (!_jumpHeld || _reachedPeak)
                    {
                        _stateMachine.ChangeState(_stateMachine.AirborneState);
                        return false;
                    }

                    return true;
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                }

                protected override void PhysicsFirstStep()
                {
                    base.PhysicsFirstStep();
                    _player.SetGravityScale(_playerData.FloatGravity);
                    _player.SetDrag(_playerData.FloatDrag);
                }

                public override string ToString()
                {
                    return "IdleState";
                }
            }
        }
    }
}



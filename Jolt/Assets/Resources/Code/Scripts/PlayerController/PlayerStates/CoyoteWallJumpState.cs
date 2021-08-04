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

                public override bool LogicUpdate()
                {
                    bool continueExecution = base.LogicUpdate();

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
                        _stateMachine.ChangeState(_stateMachine.WallJumpState);
                        return false;
                    }

                    if (timeout)
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
                    
                }
            }
        }
    }
}
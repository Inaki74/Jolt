using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public abstract class GroundedState : FullControlState
            {
                protected bool _isGrounded;
                protected bool _isMoving;
                private bool _isTouchingWall;

                public GroundedState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _player.ResetGravity();
                    _gravityActive = false;
                    _stateMachine.PreDashState.ResetAmountOfDashes();
                    _stateMachine.WallSlideState.ResetFallingGravityScale();
                    _stateMachine.WallSlideState.ResetHasClinged();
                    _player.SetAnimationBool(PlayerAnimations.Constants.GROUNDED_BOOL, true);
                }

                public override void Exit()
                {
                    base.Exit();

                    _player.ResetGravity();
                    _gravityActive = true;
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _isGrounded = _player.CheckIsGrounded();
                    _isMoving = _moveInput.x != 0;
                    _isTouchingWall = _player.CheckIsTouchingWallLeft() || _player.CheckIsTouchingWallRight();

                    if (_jumpPressed)
                    {
                        //TODO
                        if (_isTouchingWall)
                        {
                            _stateMachine.ScheduleStateChange(_stateMachine.WallSlideJumpState);
                            _player.SetAnimationBool(PlayerAnimations.Constants.GROUNDED_BOOL, false);
                            return false;
                        }

                        _stateMachine.ScheduleStateChange(_stateMachine.JumpState);
                        _player.SetAnimationBool(PlayerAnimations.Constants.GROUNDED_BOOL, false);
                        return false;
                    }

                    if (!_isGrounded)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.CoyoteJumpState);
                        return false;
                    }

                    return true;
                }
            }
        }
    }
}



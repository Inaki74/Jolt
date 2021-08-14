using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class JumpState : FullControlState
            {
                protected override Color AssociatedColor => Color.green;

                public bool ForceApplied { private get; set; }
                private bool _isGrounded;
                private bool _isTouchingWall;

                public JumpState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _player.SetGravityScale(_playerData.JumpGravity);
                    _player.SetAnimationBool(PlayerAnimations.Constants.RISING_BOOL, true);
                }

                public override void Exit()
                {
                    base.Exit();

                    ForceApplied = false;
                    _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _isTouchingWall = _player.CheckIsTouchingWallLeft() || _player.CheckIsTouchingWallRight();

                    if (ForceApplied)
                    {
                        if (_isTouchingWall)
                        {
                            _stateMachine.WallSlideJumpState.ForceApplied = true;
                            _stateMachine.ScheduleStateChange(_stateMachine.WallSlideJumpState);
                            return false;
                        }

                        if (_isGrounded)
                        {
                            _stateMachine.ScheduleStateChange(_stateMachine.IdleState);
                            return false;
                        }

                        _stateMachine.ScheduleStateChange(_stateMachine.FloatingState);
                        return false;
                    }

                    return true;
                }

                protected override void PlayerControlAction()
                {
                    base.PlayerControlAction();

                    if (!ForceApplied)
                    {
                        _player.Velocity = new Vector2(_player.Velocity.x, _playerData.JumpForce);
                        //_player.SetMovementByImpulse(Vector2.up, _playerData.JumpForce);
                        ForceApplied = true;
                    }
                }
            }
        }
    }
}
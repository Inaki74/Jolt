using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class WallAirborneState : OnWallState
            {
                protected override Color AssociatedColor => Color.black;

                private float _freefallDeformedScaleX;
                private bool _isFalling;

                public bool ForceApplied { private get; set; }

                public WallAirborneState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _isFalling = _player.Velocity.y < 0f;

                    SetAnimation();

                    _freefallDeformedScaleX = 1;
                }

                public override void Exit()
                {
                    base.Exit();

                    _player.SetScale(_playerData.PlayerPhysicsData.StandardScale);

                    _player.SetAnimationBool(PlayerAnimations.Constants.RISE_BOOL, false);
                    _player.SetAnimationBool(PlayerAnimations.Constants.FALL_BOOL, false);
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _isFalling = _player.Velocity.y < 0f;
                    bool isMovingRight = _moveInput.x > 0f;
                    bool isMovingLeft = _moveInput.x < 0f;

                    if (_isGrounded)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.IdleState);
                        return false;
                    }

                    if ((_isTouchingWallLeft && isMovingLeft) ||
                        (_isTouchingWallRight && isMovingRight))
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.WallSlideState);
                        return false;
                    }

                    SetAnimation();

                    return true;
                }

                protected override void PlayerControlAction()
                {
                    base.PlayerControlAction();

                    Freefall();
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                }

                private void Freefall()
                {
                    if (_moveInput.y < 0f)
                    {
                        _player.SetMaxFallSpeed(_playerData.FreeFallMaxFallSpeed);

                        if (_freefallDeformedScaleX > _playerData.MaxDeformedScale)
                        {
                            _freefallDeformedScaleX -= -_player.Velocity.y * Time.deltaTime * _playerData.ScaleReductionModifier;
                        }

                        Vector2 newScale = new Vector2(_freefallDeformedScaleX, 1f);
                        _player.SetScale(newScale);

                        _player.SetGravityScale(_playerData.FreeFallGravity);
                    }
                    else
                    {
                        _freefallDeformedScaleX = 1f;
                        _player.SetScale(_playerData.PlayerPhysicsData.StandardScale);
                        _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                        _player.SetMaxFallSpeed(_playerData.PlayerPhysicsData.StandardMaxFallSpeed);
                    }
                }

                private void SetAnimation()
                {
                    _player.SetAnimationBool(PlayerAnimations.Constants.RISE_BOOL, !_isFalling);
                    _player.SetAnimationBool(PlayerAnimations.Constants.FALL_BOOL, _isFalling);
                }
            }
        }
    }
}



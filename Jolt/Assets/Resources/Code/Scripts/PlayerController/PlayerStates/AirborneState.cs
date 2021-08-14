using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class AirborneState : FullControlState
            {
                protected override Color AssociatedColor => Color.red;

                private bool _isGrounded;
                private bool _isMoving;
                private bool _isTouchingWallLeft;
                private bool _isTouchingWallRight;
                private float _freefallDeformedScaleX;
                private bool _isFalling;

                public AirborneState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();

                    _freefallDeformedScaleX = _playerData.PlayerPhysicsData.StandardScale.x;
                }

                public override void Exit()
                {
                    base.Exit();

                    _player.SetScale(_playerData.PlayerPhysicsData.StandardScale);
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _isMoving = _moveInput.x != 0;
                    _isGrounded = _player.CheckIsGrounded();
                    _isFalling = _player.Velocity.y < 0f;
                    _isTouchingWallLeft = _player.CheckIsTouchingWallLeft();
                    _isTouchingWallRight = _player.CheckIsTouchingWallRight();
                    bool isTouchingWall = _isTouchingWallLeft || _isTouchingWallRight;
                    bool isMovingRight = _moveInput.x > 0f;
                    bool isMovingLeft = _moveInput.x < 0f;

                    // If it hits ground -> recoil
                    
                    if (_isGrounded)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.IdleState);
                        return false;
                    }

                    if (isTouchingWall)
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

                    if (_isFalling)
                    {
                        _player.SetAnimationBool(PlayerAnimations.Constants.RISING_BOOL, false);
                        _player.SetAnimationBool(PlayerAnimations.Constants.FALLING_BOOL, true);
                    }

                    return true;
                }

                protected override void PlayerControlAction()
                {
                    base.PlayerControlAction();

                    Freefall();

                    // TODO: WTF is this.
                    //if (_isMoving)
                    //{
                    //    if (_stateMachine.LastState == "ExitRailState")
                    //    {
                    //        _player.SetMovementXByForce(Vector2.right, _playerData.MovementSpeed * _moveInput.x);
                    //    }
                    //    else
                    //    {
                    //        _player.SetRigidbodyVelocityX(_playerData.MovementSpeed * _moveInput.x);
                    //    }
                    //}
                    //else
                    //{
                    //    if (_stateMachine.LastState != "ExitRailState")
                    //    {
                    //        _player.SetRigidbodyVelocityX(0f);
                    //    }
                    //}
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
                        _freefallDeformedScaleX = _playerData.PlayerPhysicsData.StandardScale.x;
                        _player.SetScale(_playerData.PlayerPhysicsData.StandardScale);
                        _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                        _player.SetMaxFallSpeed(_playerData.PlayerPhysicsData.StandardMaxFallSpeed);
                    }
                }
            }
        }
    }
}



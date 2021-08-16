using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class WallSlideState : OnWallState
            {
                protected override Color AssociatedColor => Color.blue;
                protected override string AnimString => PlayerAnimations.Constants.WALLFALL_BOOL;
                protected override bool _flippable => false;

                private float _currentFallingGravityScale = 0f;
                private bool _hasClinged = false;

                public WallSlideState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                    _currentFallingGravityScale = _playerData.StartingWallSlideGravity;
                }

                public override void Enter()
                {
                    base.Enter();

                    //_player.SetRigidbodyVelocityY(_playerData.InverseMultiplierOfFallSpeed);

                    if (!_hasClinged)
                    {
                        _player.Velocity = new Vector2(0f, _playerData.StartingFallSpeed);
                        _hasClinged = true;
                    }
                    
                    _player.SetGravityScale(_currentFallingGravityScale);
                    _player.SetMaxFallSpeed(_playerData.WallSlideMaxFallSpeed);
                }

                public override void Exit()
                {
                    base.Exit();

                    _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                    _player.SetMaxFallSpeed(_playerData.PlayerPhysicsData.StandardMaxFallSpeed);
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    bool isMovingRight = _moveInput.x > 0f;
                    bool isMovingLeft = _moveInput.x < 0f;

                    if (_isGrounded)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.IdleState);
                        return false;
                    }

                    if ((_isTouchingWallLeft && !isMovingLeft) ||
                        (_isTouchingWallRight && !isMovingRight))
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.WallAirborneState);
                        return false;
                    }

                    return true;
                }

                protected override void PlayerControlAction()
                {
                    base.PlayerControlAction();

                    if(_currentFallingGravityScale < _playerData.FinalWallSlideGravity)
                    {
                        _currentFallingGravityScale += Time.deltaTime * (_playerData.FinalWallSlideGravity - _playerData.StartingWallSlideGravity) / _playerData.TimeToReachFinalGravity;
                    }
                    else
                    {
                        _currentFallingGravityScale = _playerData.FinalWallSlideGravity;
                    }

                    _player.SetGravityScale(_currentFallingGravityScale);
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                }

                public void ResetFallingGravityScale()
                {
                    _currentFallingGravityScale = _playerData.StartingWallSlideGravity;
                }

                public void ResetHasClinged()
                {
                    _hasClinged = false;
                }
            }
        }
    }
}



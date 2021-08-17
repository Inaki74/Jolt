using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public abstract class FullControlState : AliveState
            {
                protected override Color AssociatedColor => Color.magenta;

                protected bool _gravityActive = true;
                protected bool _canMove = true;
                protected Vector2 _moveInput;
                protected bool _jumpPressed;
                protected bool _jumpHeld;
                protected bool _isStartingDash;
                protected bool _canDash;
                protected bool _isTouchingWallLeft;
                protected bool _isTouchingWallRight;

                public FullControlState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
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

                    _moveInput = _player.InputManager.MovementVector;
                    _jumpPressed = _player.InputManager.JumpPressed;
                    _jumpHeld = _player.InputManager.JumpHeld;
                    _isStartingDash = _player.InputManager.DashBegin;
                    _canDash = _stateMachine.DashingState.CanDash();
                    _isTouchingWallLeft = _player.CheckIsTouchingWallLeft();
                    _isTouchingWallRight = _player.CheckIsTouchingWallRight();

                    bool onLeftWallAndMovingTowardsIt = _isTouchingWallLeft && _moveInput.x < 0f;
                    bool onRightWallAndMovingTowardsIt = _isTouchingWallRight && _moveInput.x > 0f;

                    if (Flippable)
                    {
                        _player.CheckIfShouldFlip(_moveInput.x);
                    }

                    if (_isStartingDash && _canDash && !onLeftWallAndMovingTowardsIt && !onRightWallAndMovingTowardsIt)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.DashingState);
                        return false;
                    }

                    return true;
                }

                protected override void PlayerControlAction()
                {
                    base.PlayerControlAction();

                    if (_canMove)
                    {
                        _player.Velocity = new Vector2(_playerData.MovementSpeed * _moveInput.x, _player.Velocity.y);
                    }

                    if (_gravityActive)
                    {
                        _player.Gravity();
                    }

                    //_player.SetRigidbodyVelocityX(_playerData.MovementSpeed * _moveInput.x);
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();
                }
            }
        }
    }
}
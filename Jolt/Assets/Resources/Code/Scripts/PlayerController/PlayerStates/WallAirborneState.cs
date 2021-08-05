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

                public bool ForceApplied { private get; set; }

                public WallAirborneState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    _freefallDeformedScaleX = 1;
                }

                public override void Exit()
                {
                    base.Exit();
                    _player.SetScale(Vector2.one);
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
                        _stateMachine.ChangeState(_stateMachine.IdleState);
                        return false;
                    }

                    if ((_isTouchingWallLeft && isMovingLeft) ||
                        (_isTouchingWallRight && isMovingRight))
                    {
                        _stateMachine.ChangeState(_stateMachine.WallSlideState);
                        return false;
                    }

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
                        if (_freefallDeformedScaleX > _playerData.MaxDeformedScale)
                        {
                            //_freefallDeformedScaleX -= 0.01f;
                        }

                        Vector2 newScale = new Vector2(_freefallDeformedScaleX, 1f);
                        _player.SetScale(newScale);

                        _player.SetGravityScale(_playerData.FreeFallGravity);
                    }
                    else
                    {
                        _freefallDeformedScaleX = 1f;
                        _player.SetScale(Vector2.one);
                        _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                    }
                }
            }
        }
    }
}



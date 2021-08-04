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
                }

                public override void Exit()
                {
                    base.Exit();

                    ForceApplied = false;
                    _player.SetGravityScale(_playerData.PlayerPhysicsData.StandardGravity);
                    _player.SetDrag(_playerData.PlayerPhysicsData.StandardLinearDrag);
                }

                public override bool LogicUpdate()
                {
                    bool continueExecution = base.LogicUpdate();

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
                            _stateMachine.ChangeState(_stateMachine.WallSlideJumpState);
                            return false;
                        }

                        if (_isGrounded)
                        {
                            _stateMachine.ChangeState(_stateMachine.IdleState);
                            return false;
                        }

                        _stateMachine.ChangeState(_stateMachine.FloatingState);
                        return false;
                    }

                    return true;
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();

                    if (!ForceApplied)
                    {
                        _player.SetRigidbodyVelocityY(_playerData.JumpForce);
                        //_player.SetMovementByImpulse(Vector2.up, _playerData.JumpForce);
                        ForceApplied = true;
                    }
                }

                protected override void PhysicsFirstStep()
                {
                    base.PhysicsFirstStep();

                    _player.SetGravityScale(_playerData.JumpGravity);
                    _player.SetDrag(_playerData.JumpDrag);
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public abstract class OnWallState : FullControlState
            {
                protected override Color AssociatedColor => Color.magenta;

                private bool _isGrounded;
                private bool _isTouchingWallLeft;
                private bool _isTouchingWallRight;

                public OnWallState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
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

                    _isGrounded = _player.CheckIsGrounded();
                    _isTouchingWallLeft = _player.CheckIsTouchingWallLeft();
                    _isTouchingWallRight = _player.CheckIsTouchingWallRight();
                    bool isTouchingWall = _isTouchingWallLeft || _isTouchingWallRight;

                    if (_jumpPressed)
                    {
                        _stateMachine.ChangeState(_stateMachine.WallJumpState);
                        return false;
                    }

                    if (!isTouchingWall)
                    {
                        _stateMachine.ChangeState(_stateMachine.CoyoteWallJumpState);
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

                    _player.SetRigidbodyVelocityX(0f);
                }

                public override string ToString()
                {
                    return "IdleState";
                }
            }
        }
    }
}
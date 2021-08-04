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

                    _stateMachine.PreDashState.ResetAmountOfDashes();
                }

                public override bool LogicUpdate()
                {
                    bool continueExecution = base.LogicUpdate();

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
                            _stateMachine.ChangeState(_stateMachine.WallSlideJumpState);
                            return false;
                        }

                        _stateMachine.ChangeState(_stateMachine.JumpState);
                        return false;
                    }

                    if (!_isGrounded)
                    {
                        _stateMachine.ChangeState(_stateMachine.CoyoteJumpState);
                        return false;
                    }

                    return true;
                }
            }
        }
    }
}



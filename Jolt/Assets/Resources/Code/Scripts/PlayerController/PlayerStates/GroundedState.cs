using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class GroundedState : AliveState
            {
                protected Vector2 _moveInput;
                protected bool _isGrounded;
                protected bool _isStartingDash;
                protected bool _isMoving;

                public GroundedState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
                {
                }

                public override void DoChecks()
                {
                    base.DoChecks();
                    _isMoving = _moveInput.x != 0;
                }

                public override void Enter()
                {
                    base.Enter();

                    _stateMachine.PreDashState.ResetAmountOfDashes();
                }

                public override void LogicUpdate()
                {
                    base.LogicUpdate();

                    _moveInput = _player.InputManager.MovementVector;
                    _isGrounded = _player.CheckIsGrounded();
                    _isStartingDash = _player.InputManager.DashBegin;

                    //Isnt on ground -> airborne state
                    if (!_isGrounded)
                    {
                        _stateMachine.ChangeState(_stateMachine.AirborneState);
                    }
                    if (_isStartingDash)
                    {
                        _stateMachine.ChangeState(_stateMachine.PreDashState);
                    }
                    //Else remain in whichever substate
                }
            }
        }
    }
}



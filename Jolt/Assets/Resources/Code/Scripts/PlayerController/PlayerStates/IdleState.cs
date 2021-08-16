using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class IdleState : GroundedState
            {
                protected override Color AssociatedColor => Color.yellow;

                private bool _isDucking;
                private bool _isLookingUp;

                public IdleState(IPlayerStateMachine stateMachine, IPlayer player, IPlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    _player.Velocity = new Vector2(0f, _player.Velocity.y);
                    //_player.SetRigidbodyVelocityX(0f);
                }

                protected override bool StateChangeCheck()
                {
                    bool continueExecution = base.StateChangeCheck();

                    if (!continueExecution)
                    {
                        return false;
                    }

                    _isDucking = _moveInput.y < 0f;
                    _isLookingUp = _moveInput.y > 0f;

                    // Theres movement -> MoveState
                    if (_moveInput.x != 0)
                    {
                        _stateMachine.ScheduleStateChange(_stateMachine.MoveState);
                        return false;
                    }

                    return true;
                }
            }
        }
    }
}


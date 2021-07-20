using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        namespace PlayerStates
        {
            public class MoveState : GroundedState
            {

                public MoveState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
                {
                }

                public override void DoChecks()
                {
                    base.DoChecks();
                }

                public override void Enter()
                {
                    base.Enter();
                }

                public override void Exit()
                {
                    base.Exit();
                }

                public override void LogicUpdate()
                {
                    base.LogicUpdate();

                    // No movement -> idle state
                    if (_moveInput.x == 0)
                    {
                        _stateMachine.ChangeState(_stateMachine.IdleState);
                    }

                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();

                    if (_isMoving)
                        _player.SetMovementX(_playerData.movementSpeed * _moveInput.x);
                }

                public override string ToString()
                {
                    return "MoveState";
                }
            }
        }
    }
}



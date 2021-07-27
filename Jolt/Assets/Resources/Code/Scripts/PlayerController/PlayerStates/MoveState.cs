﻿using System.Collections;
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
                protected override Color AssociatedColor => Color.white;

                public MoveState(PlayerStateMachine stateMachine, Player player, PlayerData playerData) : base(stateMachine, player, playerData)
                {
                }

                public override void LogicUpdate()
                {
                    base.LogicUpdate();

                    // No movement -> idle state
                    if (!_isMoving)
                    {
                        _stateMachine.ChangeState(_stateMachine.IdleState);
                    }
                }

                public override void PhysicsUpdate()
                {
                    base.PhysicsUpdate();

                    if (_isMoving)
                    {
                        _player.SetMovementX(_playerData.movementSpeed * _moveInput.x);
                    }
                }

                public override string ToString()
                {
                    return "MoveState";
                }
            }
        }
    }
}



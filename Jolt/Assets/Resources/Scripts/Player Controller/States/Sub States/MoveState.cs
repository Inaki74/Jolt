using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(moveInput.x == 0)
        {
            stateMachine.ChangeState(player.IdleState);
        }
            
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if(isMoving)
            player.SetMovementX(playerData.movementSpeed * moveInput.x);
    }

    public override string ToString()
    {
        return "MoveState";
    }
}

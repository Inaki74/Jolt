using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilState : GroundedState
{
    private float timeToChange = 0.2f;
    private float currentTime;

    public RecoilState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
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

        currentTime = Time.time;

        // After enough time elapses, change to the appropiate state
        if(currentTime - enterTime > timeToChange)
        {
            // Movement -> move
            if(moveInput.x != 0)
            {
                stateMachine.ChangeState(stateMachine.MoveState);
            }
            // No Movement -> idle
            else
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (isMoving)
        {
            player.SetMovementX(playerData.movementSpeed * moveInput.x);
        }
        else
        {
            player.SetMovementX(0f);
        }
    }

    public override string ToString()
    {
        return "RecoilState";
    }
}

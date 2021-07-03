using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : PlayerState
{
    private float currentTime;

    // Instantiate particles
    // Move player to last checkpoint (but here we will have only one checkpoint, so skip)
    // Reset objects (but here they are immutable so skip)

    public DeadState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        player.SetGravityScale(0f);
        player.SetMovementX(0f); player.SetMovementY(0f);
        player.SetActivePhysicsCollider(false);
        player.InstantiateDeathParticles();
    }

    public override void Exit()
    {
        base.Exit();

        player.ResetPosition();
        player.SetActivePhysicsCollider(true);
        player.SetGravityScale(1f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        currentTime = Time.time;

        if(currentTime - enterTime > playerData.deadTimer)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override string ToString()
    {
        return "DeadState";
    }
}

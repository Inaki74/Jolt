using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductorState : PlayerState
{

    public ConductorState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetActiveSpriteRenderer(false);
        player.SetGravityScale(0f);
        player.PreDashState.ResetAmountOfDashes();
    }

    public override void Exit()
    {
        base.Exit();
        player.SetActiveSpriteRenderer(true);
        player.SetGravityScale(1f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

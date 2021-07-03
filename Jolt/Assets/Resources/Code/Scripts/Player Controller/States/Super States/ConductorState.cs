﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductorState : AliveState
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
        player.SetGravityScale(0f);
        stateMachine.PreDashState.ResetAmountOfDashes();
    }

    public override void Exit()
    {
        base.Exit();
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

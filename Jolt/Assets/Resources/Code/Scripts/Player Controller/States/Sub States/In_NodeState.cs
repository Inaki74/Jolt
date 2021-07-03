using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class In_NodeState : ConductorState
{
    private bool isStartingDash;

    public In_NodeState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
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
        //base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        isStartingDash = player.InputManager.DashBegin;

        if (isStartingDash)
        {
            stateMachine.ChangeState(stateMachine.ExitNodeState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.SetPosition(player.GetNodeInfo().transform.position);
    }

    public override string ToString()
    {
        return "In_NodeState";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveState : PlayerState
{
    private bool isAlive;
    public AliveState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
    {
    }

    public override void Enter()
    {
        base.Enter();

        isAlive = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        isAlive = !player.CheckIfDead();

        if (!isAlive)
        {
            stateMachine.ChangeState(player.DeadState);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected PlayerData playerData;
    protected Color associatedColor;

    public PlayerState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor)
    {
        this.stateMachine = stateMachine;
        this.player = player;
        this.playerData = playerData;
        this.associatedColor = associatedColor;
    }

    public virtual void Enter()
    {
        DoChecks();
    }

    public virtual void Exit()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void DoChecks()
    {

    }
}

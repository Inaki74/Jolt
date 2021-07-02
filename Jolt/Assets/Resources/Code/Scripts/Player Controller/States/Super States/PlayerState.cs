using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected PlayerData playerData;
    protected Color associatedColor; // string animBool

    protected float enterTime;

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

        player.Sr.color = associatedColor;
        enterTime = Time.time;
        //Debug.Log(ToString());
    }

    public virtual void Exit()
    {

    }

    public virtual void LogicUpdate()
    {
    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks()
    {

    }

}

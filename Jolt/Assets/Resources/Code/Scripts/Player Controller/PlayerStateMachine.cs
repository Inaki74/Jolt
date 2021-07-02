using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }

    public string LastState { get; private set; }

    public MoveState MoveState { get; private set; }
    public IdleState IdleState { get; private set; }
    public AirborneState AirborneState { get; private set; }
    public RecoilState RecoilState { get; private set; }
    public PreDashState PreDashState { get; private set; }
    public DashingState DashingState { get; private set; }
    public In_NodeState InNodeState { get; private set; }
    public ExitNodeState ExitNodeState { get; private set; }
    public In_RailState InRailState { get; private set; }
    public ExitRailState ExitRailState { get; private set; }
    public DeadState DeadState { get; private set; }

    public PlayerStateMachine(Player player, PlayerData playerData)
    {
        MoveState = new MoveState(this, player, playerData, Color.white);
        IdleState = new IdleState(this, player, playerData, Color.yellow);
        AirborneState = new AirborneState(this, player, playerData, Color.red);
        RecoilState = new RecoilState(this, player, playerData, Color.magenta);
        PreDashState = new PreDashState(this, player, playerData, Color.gray);
        DashingState = new DashingState(this, player, playerData, Color.cyan);
        InNodeState = new In_NodeState(this, player, playerData, Color.clear);
        ExitNodeState = new ExitNodeState(this, player, playerData, Color.magenta);
        InRailState = new In_RailState(this, player, playerData, Color.cyan);
        ExitRailState = new ExitRailState(this, player, playerData, Color.magenta);
        DeadState = new DeadState(this, player, playerData, Color.clear);
    }

    public void Initialize()
    {
        PlayerState startingState = IdleState;

        TransitionState(startingState);
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        LastState = CurrentState.ToString();
        TransitionState(newState);
    }

    public string GetState()
    {
        return CurrentState.ToString();
    }

    private void TransitionState(PlayerState newState)
    {
        CurrentState = newState;
        newState.Enter();
    }
}

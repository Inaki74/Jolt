using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreDashState : AliveState
{
    private bool isDashStarted;
    private float currentTime;
    private int amountOfDashes;

    public PreDashState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
    {
        ResetAmountOfDashes();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        isDashStarted = true;
        Time.timeScale = playerData.timeSlow;
        player.DrawCircle(player.InputManager.InitialDashPoint, playerData.circleRadius);
        DecreaseAmountOfDashes();
        //Time.fixedDeltaTime = 0.1f * 0.02f; Works but doubles the CPU usage. Use RigidBodies with interpolate instead
    }

    public override void Exit()
    {
        base.Exit();

        player.DeactivateArrowRendering();
        player.DeactivateCircleRendering();
        Time.timeScale = 1f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        currentTime = Time.time;
        isDashStarted = player.InputManager.DashBegin && (currentTime - enterTime < playerData.preDashTimeOut);

        player.SetDashVectors(player.InputManager.InitialDashPoint, player.InputManager.FinalDashPoint);
        player.SetArrowRendering();

        //Cant be Cancelled, go to dashing when stopped pressing or after timeout
        if (!isDashStarted)
        {
            // transition to dashing
            stateMachine.ChangeState(player.DashingState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override string ToString()
    {
        return "PreDashState";
    }

    public bool CanDash()
    {
        return amountOfDashes > 0;
    }

    public void ResetAmountOfDashes()
    {
        amountOfDashes = playerData.amountOfDashes;
    }

    public void DecreaseAmountOfDashes()
    {
        amountOfDashes--;
    }
}

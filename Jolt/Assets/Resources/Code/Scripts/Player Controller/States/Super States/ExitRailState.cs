using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitRailState : AliveState
{
    public Vector2 exitVector;

    public float ExitSpeed { private get; set; }

    private Vector2 moveInput;
    private bool isGrounded;
    private bool isStartingDash;

    public ExitRailState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        exitVector.Normalize();
        player.SetForceToGivenVector(exitVector, ExitSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        isGrounded = player.CheckIsGrounded();
        moveInput = player.InputManager.MovementVector;
        isStartingDash = player.InputManager.DashBegin;

        if (isGrounded)
        {
            stateMachine.ChangeState(stateMachine.RecoilState);
        }
        else if(Mathf.Abs(player.GetCurrentVelocity().x) < 0.2f)
        {
            stateMachine.ChangeState(stateMachine.AirborneState);
        }else if (isStartingDash && stateMachine.PreDashState.CanDash())
        {
            stateMachine.ChangeState(stateMachine.PreDashState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        //Exit right
        if(exitVector.x > 0)
        {
            if(player.GetCurrentVelocity().x < playerData.movementSpeed || moveInput.x < 0)
                player.SetMovementXByForce(moveInput, playerData.movementSpeed + ExitSpeed);
        }
        //Exit left
        else
        {
            if (player.GetCurrentVelocity().x > -playerData.movementSpeed || moveInput.x > 0)
                player.SetMovementXByForce(moveInput, playerData.movementSpeed + ExitSpeed);
        }

    }

    public override string ToString()
    {
        return "ExitRailState";
    }
}

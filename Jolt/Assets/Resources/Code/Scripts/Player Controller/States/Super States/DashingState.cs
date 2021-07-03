using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingState : AliveState
{
    private bool isGrounded;
    private Vector2 moveInput;
    private float currentTime;
    private bool isTouchingNode;
    private bool isTouchingRail;

    private bool playOnce;

    public DashingState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        playOnce = true;
        player.SetGravityScale(0f);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetGravityScale(1f);
        player.SetMovementX(0f); player.SetMovementY(0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        
        isGrounded = player.CheckIsGrounded();
        moveInput = player.InputManager.MovementVector;
        isTouchingNode = player.CheckIsTouchingNode();
        isTouchingRail = player.CheckIsTouchingRail();

        if (isTouchingNode)
        {
            stateMachine.ChangeState(stateMachine.InNodeState);
        }
        else if (isTouchingRail)
        {
            stateMachine.ChangeState(stateMachine.InRailState);
        }

        if (currentTime - enterTime > playerData.dashTimeOut)
        {
            if (isGrounded)
            {
                if(moveInput.x != 0)
                {
                    stateMachine.ChangeState(stateMachine.MoveState);
                }
                else
                {
                    stateMachine.ChangeState(stateMachine.IdleState);
                }
            }
            else
            {
                stateMachine.ChangeState(stateMachine.AirborneState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        currentTime = Time.time;

        if (playOnce)
        {
            player.SetDashMovement(playerData.dashSpeed);

            playOnce = false;
        }
        
    }

    public override string ToString()
    {
        return "DashingState";
    }
}

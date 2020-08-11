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

        currentTime = Time.time;
        isGrounded = player.CheckIsGrounded();
        moveInput = player.InputManager.MovementVector;
        isTouchingNode = player.CheckIsTouchingNode();
        isTouchingRail = player.CheckIsTouchingRail();


        if (currentTime - enterTime > playerData.dashTimeOut)
        {
            if (isGrounded)
            {
                if(moveInput.x != 0)
                {
                    stateMachine.ChangeState(player.MoveState);
                }
                else
                {
                    stateMachine.ChangeState(player.IdleState);
                }
            }
            else
            {
                stateMachine.ChangeState(player.AirborneState);
            }
        }

        if (isTouchingNode)
        {
            stateMachine.ChangeState(player.InNodeState);
        }
        else if (isTouchingRail)
        {
            stateMachine.ChangeState(player.InRailState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

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

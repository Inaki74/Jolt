using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreDashState : PlayerState
{
    private Vector2 moveInput;
    private bool isGrounded;
    private bool isDashStarted;
    private float currentTime;

    public PreDashState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
    {
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
        //Time.fixedDeltaTime = 0.1f * 0.02f; Works but doubles the CPU usage. Use RigidBodies with interpolate instead
    }

    public override void Exit()
    {
        base.Exit();

        Time.timeScale = 1f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        currentTime = Time.time;
        isDashStarted = player.InputManager.DashBegin; // && (currentTime - enterTime > playerData.dashTimeOut);
        isGrounded = player.CheckIsGrounded();
        moveInput = player.InputManager.MovementVector;
        

        //Cancelled (there must be a "dead zone" to allow cancellation and a timeout)
        if (!isDashStarted)
        {
            if (isGrounded)
            {
                //Grounded and idle -> idle
                if(moveInput.x == 0)
                {
                    stateMachine.ChangeState(player.IdleState);
                }
                //Grounded and moving -> move
                else
                {
                    stateMachine.ChangeState(player.MoveState);
                }
            }
            //!grounded -> airborne
            else
            {
                stateMachine.ChangeState(player.AirborneState);
            }
        }
        else
        {
            //Isnt cancelled
            //Pre dash logic to dash

            //Show arrow which updates with mouse position (starts outside of "dead zone")

            //Once released, transition to dash state
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
}

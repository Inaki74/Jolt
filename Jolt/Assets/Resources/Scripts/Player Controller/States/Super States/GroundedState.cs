using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : PlayerState
{
    protected Vector2 moveInput;
    protected bool isGrounded;
    protected bool isStartingDash;

    public GroundedState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
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
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        moveInput = player.InputManager.MovementVector;
        isGrounded = player.CheckIsGrounded();
        isStartingDash = player.InputManager.DashBegin;

        //Isnt on ground -> airborne state
        if (!isGrounded)
        {
            stateMachine.ChangeState(player.AirborneState);
        }
        if (isStartingDash)
        {
            stateMachine.ChangeState(player.PreDashState);
        }
        //Else remain in whichever substate
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

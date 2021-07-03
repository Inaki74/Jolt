using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneState : AliveState
{
    private Vector2 _moveInput;
    private bool isGrounded;
    private bool isStartingDash;
    private bool isMoving;

    public AirborneState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isMoving = _moveInput.x != 0;
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

        _moveInput = player.InputManager.MovementVector;
        isGrounded = player.CheckIsGrounded();
        isStartingDash = player.InputManager.DashBegin;

        // If it hits ground -> recoil
        if (isGrounded)
        {
            stateMachine.ChangeState(stateMachine.RecoilState);
        }
        else if(isStartingDash && stateMachine.PreDashState.CanDash())
        {
            stateMachine.ChangeState(stateMachine.PreDashState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (isMoving)
        {
            if (stateMachine.LastState == "ExitRailState")
                player.SetMovementXByForce(Vector2.right, playerData.movementSpeed * _moveInput.x);
            else
                player.SetMovementX(playerData.movementSpeed * _moveInput.x);
        }
        else
        {
            if (stateMachine.LastState != "ExitRailState")
                player.SetMovementX(0f);
        }
    }

    public override string ToString()
    {
        return "AirborneState";
    }
}

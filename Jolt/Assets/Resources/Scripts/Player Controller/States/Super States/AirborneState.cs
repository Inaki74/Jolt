using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneState : PlayerState
{
    private Vector2 _moveInput;
    private bool isGrounded;
    private bool isStartingDash;

    public AirborneState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
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

        _moveInput = player.InputManager.MovementVector;
        isGrounded = player.CheckIsGrounded();
        isStartingDash = player.InputManager.DashBegin;

        // If it hits ground -> recoil
        if (isGrounded)
        {
            stateMachine.ChangeState(player.RecoilState);
        }
        else if(isStartingDash)
        {
            stateMachine.ChangeState(player.PreDashState);
        }
        else
        {
            player.SetMovementX(playerData.movementSpeed * _moveInput.x);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override string ToString()
    {
        return "AirborneState";
    }
}

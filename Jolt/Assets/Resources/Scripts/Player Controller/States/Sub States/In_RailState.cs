using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class In_RailState : ConductorState
{
    private RailController currentRail;

    private Vector2 nextPath;

    private float t;

    private float speed;

    private bool exiting;
    private bool reachedPath;

    public In_RailState(PlayerStateMachine stateMachine, Player player, PlayerData playerData, Color associatedColor) : base(stateMachine, player, playerData, associatedColor)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        playerData.allPaths.Clear();
        exiting = false;
        nextPath = player.transform.position;
        currentRail = player.GetRailInfo();

        t = 0f;
        speed = currentRail.railSpeed;
    }

    public override void Exit()
    {
        base.Exit();

        Vector2[] aux = playerData.allPaths.ToArray();

        //Debug.Log("PreLast: (" + aux[aux.Length - 2].x + " , " + aux[aux.Length - 2].y + ") , Last: (" + nextPath.x + " , " + nextPath.y + ")");
        player.ExitRailState.exitVector = nextPath - aux[aux.Length - 2];
        player.ExitRailState.ExitSpeed = speed;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        reachedPath = player.CheckHasReachedPoint(nextPath);

        if (t < 1 && reachedPath)
        {
            t += Time.deltaTime * speed/10;

            nextPath = SplineHelperFunctions.SplineCurve(currentRail.ControlPoints.Length - 1, 0, t, currentRail.ControlPoints);
            if (t > 0.8)
                playerData.allPaths.Add(nextPath);
        }
        else if (t >= 1)
        {
            t = 0;
            exiting = true;
        }

        player.MoveTowardsVector(nextPath, speed);

        if (exiting)
        {
            stateMachine.ChangeState(player.ExitRailState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        //Follows speed according to points
        //if (t < 1)
        //{
        //    t += Time.fixedDeltaTime * speed;

        //    Vector3 aux = SplineHelperFunctions.SplineCurve(currentRail.ControlPoints.Length - 1, 0, t, currentRail.ControlPoints);

        //    nextPath.Set(aux.x, aux.y);

        //    player.SetPosition(nextPath);
        //    player.CheckHasReachedPoint(nextPath);
        //}
        //else
        //{
        //    t = 0f;

        //    exiting = true;
        //}
    }

    public override string ToString()
    {
        return "InRailState";
    }
}

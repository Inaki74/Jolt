using System.Collections.Generic;
using UnityEngine;

namespace Jolt.PlayerController
{
    public interface IPlayerData
    {
        IPlayerPhysicsData PlayerPhysicsData { get; }
        float MovementSpeed { get; }
        float TimeSlow { get; }
        float PreDashTimeOut { get; }
        int AmountOfDashes { get; }
        float CircleRadius { get; }
        float DashTimeOut { get; }
        float DashSpeed { get; }
        float CheckWallRadius { get; }
        float CheckGroundRadius { get; }
        LayerMask WhatIsGround { get; }
        List<Vector2> AllPaths { get; }
        float DeadTimer { get; }
        Vector2 LastCheckpoint { get; }
        float RecoilTimer { get;  }
        float FreeFallGravity { get; }
        float MaxDeformedScale { get; }
        float JumpForce { get; }
        float JumpGravity { get; }
        float JumpDrag { get; }
        float JumpCoyoteTiming { get; }
        float FloatGravity { get; }
        float FloatDrag { get; }
        float WallSlideGravity { get; }
        float WallSlideDrag { get; }
        float WallJumpGravity { get; }
        float WallJumpDrag { get; }
        float WallJumpForceHorizontal { get; }
        float WallJumpForceVerticalRatioWithHorizontal { get; }
        float WallJumpCoyoteTiming { get; }
        float InverseMultiplierOfFallSpeed { get; }
    }
}
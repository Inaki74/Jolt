﻿using System.Collections.Generic;
using UnityEngine;

namespace Jolt.PlayerController
{
    public interface IPlayerData
    {
        float MovementSpeed { get; }
        float TimeSlow { get; }
        float PreDashTimeOut { get; }
        int AmountOfDashes { get; }
        float CircleRadius { get; }
        float DashTimeOut { get; }
        float DashSpeed { get; }
        float CheckGroundRadius { get; }
        LayerMask WhatIsGround { get; }
        List<Vector2> AllPaths { get; }
        float DeadTimer { get; }
        Vector2 LastCheckpoint { get; }
    }
}
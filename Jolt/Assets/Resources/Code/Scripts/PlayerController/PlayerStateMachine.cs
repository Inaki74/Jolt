using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        using PlayerStates;

        public class PlayerStateMachine : IPlayerStateMachine
        {
            public PlayerState CurrentState { get; private set; }

            public string LastState { get; private set; }

            public MoveState MoveState { get; private set; }
            public IdleState IdleState { get; private set; }
            public AirborneState AirborneState { get; private set; }
            public RecoilState RecoilState { get; private set; }
            public PreDashState PreDashState { get; private set; }
            public DashingState DashingState { get; private set; }
            public In_NodeState InNodeState { get; private set; }
            public ExitNodeState ExitNodeState { get; private set; }
            public In_RailState InRailState { get; private set; }
            public ExitRailState ExitRailState { get; private set; }
            public DeadState DeadState { get; private set; }
            public JumpState JumpState { get; private set; }
            public FallingState FallingState { get; private set; }

            public PlayerStateMachine(IPlayer player, IPlayerData playerData)
            {
                MoveState = new MoveState(this, player, playerData);
                IdleState = new IdleState(this, player, playerData);
                AirborneState = new AirborneState(this, player, playerData);
                RecoilState = new RecoilState(this, player, playerData);
                PreDashState = new PreDashState(this, player, playerData);
                DashingState = new DashingState(this, player, playerData);
                InNodeState = new In_NodeState(this, player, playerData);
                ExitNodeState = new ExitNodeState(this, player, playerData);
                InRailState = new In_RailState(this, player, playerData);
                ExitRailState = new ExitRailState(this, player, playerData);
                DeadState = new DeadState(this, player, playerData);
                JumpState = new JumpState(this, player, playerData);
                FallingState = new FallingState(this, player, playerData);
            }

            public void Initialize()
            {
                PlayerState startingState = IdleState;

                TransitionState(startingState);
            }

            public void ChangeState(PlayerState newState)
            {
                CurrentState.Exit();
                LastState = CurrentState.ToString();
                TransitionState(newState);
            }

            public string GetState()
            {
                return CurrentState.ToString();
            }

            private void TransitionState(PlayerState newState)
            {
                CurrentState = newState;
                newState.Enter();
            }
        }
    }
}


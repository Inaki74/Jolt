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
            public FloatingState FloatingState { get; private set; }
            public CoyoteJumpState CoyoteJumpState { get; private set; }
            public WallSlideState WallSlideState { get; private set; }
            public WallJumpState WallJumpState { get; private set; }
            public CoyoteWallJumpState CoyoteWallJumpState { get; private set; }
            public WallSlideFloatingState WallSlideFloatingState { get; private set; }
            public WallSlideJumpState WallSlideJumpState { get; private set; }
            public WallAirborneState WallAirborneState { get; private set; }

            private PlayerState _nextState;
            private bool _stateChanged = false;

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
                FloatingState = new FloatingState(this, player, playerData);
                CoyoteJumpState = new CoyoteJumpState(this, player, playerData);
                WallSlideState = new WallSlideState(this, player, playerData);
                WallJumpState = new WallJumpState(this, player, playerData);
                CoyoteWallJumpState = new CoyoteWallJumpState(this, player, playerData);
                WallSlideFloatingState = new WallSlideFloatingState(this, player, playerData);
                WallSlideJumpState = new WallSlideJumpState(this, player, playerData);
                WallAirborneState = new WallAirborneState(this, player, playerData);
            }

            public void Initialize()
            {
                PlayerState startingState = IdleState;

                TransitionState(startingState);
            }

            public void ChangeState()
            {
                if (!_stateChanged)
                {
                    return;
                }

                CurrentState.Exit();
                LastState = CurrentState.ToString();
                TransitionState(_nextState);
            }

            public string GetState()
            {
                return CurrentState.ToString();
            }

            private void TransitionState(PlayerState newState)
            {
                CurrentState = newState;
                newState.Enter();

                _stateChanged = false;
            }

            public void ScheduleStateChange(PlayerState newState)
            {
                _stateChanged = true;
                _nextState = newState;
            }

            public void ForceStateChange(PlayerState newState)
            {
                //TODO if needed
            }
        }
    }
}


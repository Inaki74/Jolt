using Jolt.PlayerController.PlayerStates;

namespace Jolt.PlayerController
{
    public interface IPlayerStateMachine
    {
        PlayerState CurrentState { get; }
        PlayerState NextState { get; }
        string LastState { get; }
        MoveState MoveState { get; }
        IdleState IdleState { get; }
        AirborneState AirborneState { get; }
        RecoilState RecoilState { get; }
        DashingState DashingState { get; }
        DashFloatingState DashFloatingState { get; }
        In_NodeState InNodeState { get; }
        ExitNodeState ExitNodeState { get; }
        In_RailState InRailState { get; }
        ExitRailState ExitRailState { get; }
        DeadState DeadState { get; }
        JumpState JumpState { get; }
        FloatingState FloatingState { get; }
        CoyoteJumpState CoyoteJumpState { get; }
        WallSlideState WallSlideState { get; }
        WallJumpState WallJumpState { get; }
        WallSlideFloatingState WallSlideFloatingState { get; }
        WallSlideJumpState WallSlideJumpState { get; }
        CoyoteWallJumpState CoyoteWallJumpState { get; }
        WallAirborneState WallAirborneState { get; }

        void ChangeState();
        void ScheduleStateChange(PlayerState newState);
        void ForceStateChange(PlayerState newState);
        string GetState();
        void Initialize();
    }
}
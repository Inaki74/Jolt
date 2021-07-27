using Jolt.PlayerController.PlayerStates;

namespace Jolt.PlayerController
{
    public interface IPlayerStateMachine
    {
        PlayerState CurrentState { get; }
        string LastState { get; }
        MoveState MoveState { get; }
        IdleState IdleState { get; }
        AirborneState AirborneState { get; }
        RecoilState RecoilState { get; }
        PreDashState PreDashState { get; }
        DashingState DashingState { get; }
        In_NodeState InNodeState { get; }
        ExitNodeState ExitNodeState { get; }
        In_RailState InRailState { get; }
        ExitRailState ExitRailState { get; }
        DeadState DeadState { get; }

        void ChangeState(PlayerState newState);
        string GetState();
        void Initialize();
    }
}
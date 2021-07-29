using UnityEngine;

namespace Jolt.PlayerController
{
    public interface IPlayerInputManager
    {
        Vector2 MovementVector { get; set; }
        bool DashBegin { get; set; }
        Vector3 InitialDashPoint { get; set; }
        Vector3 FinalDashPoint { get; set; }
        bool JumpPressed { get; set; }
    }
}
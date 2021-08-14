using UnityEngine;

namespace Jolt.PlayerController
{
    public interface IPlayerPhysicsData
    {
        Vector2 StandardScale { get; }
        float StandardGravity { get; }
        float StandardMaxFallSpeed { get; }
    }
}
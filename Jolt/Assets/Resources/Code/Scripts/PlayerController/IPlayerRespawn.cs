using UnityEngine;

namespace Jolt.PlayerController
{
    public interface IPlayerRespawn
    {
        Vector2 PlayerRespawnLocation { get; set; }

        void RespawnPlayer();
    }
}
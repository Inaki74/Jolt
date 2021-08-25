using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        public class PlayerRespawn : MonoBehaviour, IPlayerRespawn
        {
            public Vector2 PlayerRespawnLocation { get; set; }

            public void RespawnPlayer()
            {
                transform.position = PlayerRespawnLocation;
            }
        }
    }
}



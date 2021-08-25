using UnityEngine;

namespace Jolt
{
    namespace LevelSections
    {
        using PlayerController;

        public abstract class ISectionTransitionController : MonoBehaviour
        {
            public abstract string FromID { get; }
            public abstract string ToID { get; }

            public abstract Transform RespawnTransform { get; }

            public delegate void DetectedPlayerTransitioning(string fromId, string toId);
            public static event DetectedPlayerTransitioning OnPlayerTransitionedSection;

            protected abstract bool DetectPlayer();

            private void Update()
            {
                if (DetectPlayer())
                {
                    IPlayerRespawn playerRespawn = FindObjectOfType<PlayerRespawn>();

                    playerRespawn.PlayerRespawnLocation = RespawnTransform.position;

                    OnPlayerTransitionedSection.Invoke(FromID, ToID);
                }
            }
        }
    }
}

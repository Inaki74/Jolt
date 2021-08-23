using UnityEngine;

namespace Jolt
{
    namespace LevelSections
    {
        public abstract class ISectionTransitionController : MonoBehaviour
        {
            public abstract string FromID { get; }
            public abstract string ToID { get; }

            public delegate void DetectedPlayerTransitioning(string fromId, string toId);
            public static event DetectedPlayerTransitioning OnPlayerTransitionedSection;

            protected abstract bool DetectPlayer();

            private void Update()
            {
                if (DetectPlayer())
                {
                    OnPlayerTransitionedSection.Invoke(FromID, ToID);
                }
            }
        }
    }
}

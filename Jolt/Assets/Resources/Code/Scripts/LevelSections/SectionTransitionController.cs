using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace LevelSections
    {
        public class SectionTransitionController : ISectionTransitionController
        {
            [SerializeField] private string _from;
            [SerializeField] private string _to;

            public override string FromID { get => _from; }
            public override string ToID { get => _to; }

            protected override bool DetectPlayer()
            {
                // Detect player via raycast.

                return false;
            }
        }
    }
}



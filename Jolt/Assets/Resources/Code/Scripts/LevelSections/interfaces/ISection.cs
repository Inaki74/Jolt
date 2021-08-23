using System.Collections.Generic;

namespace Jolt
{
    namespace LevelSections
    {
        public interface ISection
        {
            string ID { get; }
            SectionBoundaries SectionBoundaries { get; }
            List<ISectionTransitionController> SectionTransitioners { get; }
            List<IGameObjectPlacer> GameObjectPlacers { get; }

            void Enter();
            void Exit();
        }

        public struct SectionBoundaries
        {
            public float Height;
            public float Width;
        }
    }
}
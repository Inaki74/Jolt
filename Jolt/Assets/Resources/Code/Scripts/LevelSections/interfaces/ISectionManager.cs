using System.Collections.Generic;

namespace Jolt
{
    namespace LevelSections
    {
        public interface ISectionManager
        {
            List<ISection> Sections { get; }
            ISection CurrentSection { get; set; }

            void OnPlayerTransitionedSection(string toId);
        }
    }
}
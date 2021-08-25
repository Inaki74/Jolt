
namespace Jolt
{
    namespace LevelSections
    {
        public interface ISectionCameraAdjuster
        {
            //CinemachineVirtualCamera CinemachineCamera { get; set; }

            void Transition(ISection sectionToTransition, ISection sectionTransitionedFrom);
        }
    }
}

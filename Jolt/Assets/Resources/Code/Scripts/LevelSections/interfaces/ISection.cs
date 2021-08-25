using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Jolt
{
    namespace LevelSections
    {
        public interface ISection
        {
            string ID { get; }
            CinemachineVirtualCamera Camera { get; }
            List<ISectionTransitionController> SectionTransitioners { get; }
            List<IGameObjectPlacer> GameObjectPlacers { get; }

            void Enter();
            void Exit();
        }
    }
}
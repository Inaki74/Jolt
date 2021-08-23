using UnityEngine;

namespace Jolt
{
    namespace LevelSections
    {
        public interface IGameObjectPlacer
        {
            GameObject PrefabToPlace { get; }

            void SpawnPrefab();
            void DespawnPrefab();
        }
    }
}
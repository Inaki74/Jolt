using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace LevelSections
    {
        public class GameObjectPlacer : MonoBehaviour, IGameObjectPlacer
        {
            [SerializeField] private GameObject _prefabToPlace;

            public GameObject PrefabToPlace { get => _prefabToPlace; }

            public void DespawnPrefab()
            {
                // Loads prefab from memory and places it at its location.

                throw new System.NotImplementedException();
            }

            public void SpawnPrefab()
            {
                // Unloads prefab from memory.

                throw new System.NotImplementedException();
            }
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Jolt
{
    namespace LevelSections
    {
        using PlayerController;

        public class Section : MonoBehaviour, ISection
        {
            public const float CELLS_UNIT_RATIO = 2f / 3f;

            [SerializeField] private string _id;
            [SerializeField] private CinemachineVirtualCamera _camera;
            private PolygonCollider2D _sectionBoundaries;
            private List<IGameObjectPlacer> _gameObjectPlacers = new List<IGameObjectPlacer>();
            private List<ISectionTransitionController> _sectionTransitionControllers = new List<ISectionTransitionController>();

            [SerializeField] private GameObject _sectionTransitionControllersContainer;
            [SerializeField] private GameObject _gameObjectPlacersContainer;

            [Header("Height and Width in amount of cells")]
            [SerializeField] private int _cellsHeight;
            [SerializeField] private int _cellsWidth;

            public string ID { get => _id; }
            public CinemachineVirtualCamera Camera { get => _camera; }
            public List<ISectionTransitionController> SectionTransitioners { get => _sectionTransitionControllers; }
            public List<IGameObjectPlacer> GameObjectPlacers { get => _gameObjectPlacers; }

            public void Enter()
            {
                // Loads all game objects.
                // Turn on transitioners.
                Camera.Follow = FindObjectOfType<Player>().transform; // assuming there is one and only one
                Camera.enabled = true;

                SetTransitioners(true);
            }

            public void Exit()
            {
                // Unloads all game objects.
                // Turn off transitioners.
                Camera.Follow = null;
                Camera.enabled = false;

                SetTransitioners(false);
            }

            // Start is called before the first frame update
            void Start()
            {
                GetBoundaries();
                GetGameObjectPlacers();
                GetTransitionControllers();
            }

            private void SetTransitioners(bool set)
            {
                foreach(ISectionTransitionController controller in SectionTransitioners)
                {
                    controller.enabled = set;
                }
            }

            private void ActivateGameObjects()
            {

            }

            private void GetTransitionControllers()
            {
                foreach(ISectionTransitionController controller in _gameObjectPlacersContainer.GetComponentsInChildren<ISectionTransitionController>())
                {
                    _sectionTransitionControllers.Add(controller);
                }
            }

            private void GetGameObjectPlacers()
            {
                foreach (IGameObjectPlacer placer in _gameObjectPlacersContainer.GetComponentsInChildren<IGameObjectPlacer>())
                {
                    _gameObjectPlacers.Add(placer);
                }
            }

            private void GetBoundaries()
            {
                _sectionBoundaries = GetComponent<PolygonCollider2D>();

                float height = TransformHeightToFloatValue(_cellsHeight)/2;
                float width = TransformWidthToFloatValue(_cellsWidth)/2;

                Vector2 topLeft = new Vector2(-width, height);
                Vector2 topRight = new Vector2(width, height);
                Vector2 bottomRight = new Vector2(width, -height);
                Vector2 bottomLeft = new Vector2(-width, -height);

                Vector2[] path = new Vector2[4];
                path[0] = topLeft;
                path[1] = topRight;
                path[2] = bottomRight;
                path[3] = bottomLeft;

                _sectionBoundaries.pathCount = 1;
                _sectionBoundaries.SetPath(0, path);
            }

            private float TransformHeightToFloatValue(int cells) => cells * CELLS_UNIT_RATIO;

            private float TransformWidthToFloatValue(int cells) => cells * CELLS_UNIT_RATIO;

            // Gizmos
            private float _gizmosHeight;
            private float _gizmosWidth;
            [SerializeField] private bool _resetHeightAndWidth = false;

            private void OnDrawGizmos()
            {
                if (!_resetHeightAndWidth)
                {
                    _resetHeightAndWidth = true;

                    _gizmosHeight = TransformHeightToFloatValue(_cellsHeight);
                    _gizmosWidth = TransformWidthToFloatValue(_cellsWidth);
                }

                Vector3 center = new Vector3(transform.position.x - (_cellsWidth % 2 == 0 ? 0 : 1) * CELLS_UNIT_RATIO / 2, transform.position.y - (_cellsHeight % 2 == 0 ? 0 : 1) * CELLS_UNIT_RATIO / 2, 0f);
                Vector3 size = new Vector3(_gizmosWidth, _gizmosHeight, 0f);

                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(center, size);
            }
        }
    }
}



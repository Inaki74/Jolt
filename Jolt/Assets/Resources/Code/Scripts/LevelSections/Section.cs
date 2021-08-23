using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace LevelSections
    {
        public class Section : MonoBehaviour, ISection
        {
            public const float CELLS_UNIT_RATIO = 2f / 3f;

            [SerializeField] private string _id;
            [SerializeField] private List<ISectionTransitionController> _sectionTransitionControllers = new List<ISectionTransitionController>();
            [SerializeField] private GameObject _gameObjectPlacersContainer;
            private SectionBoundaries _sectionBoundaries;

            [Header("Height and Width in amount of cells")]
            [SerializeField] private int _cellsHeight;
            [SerializeField] private int _cellsWidth;

            public string ID { get => _id; }
            public SectionBoundaries SectionBoundaries { get => _sectionBoundaries; }
            public List<ISectionTransitionController> SectionTransitioners { get => _sectionTransitionControllers; }
            public List<IGameObjectPlacer> GameObjectPlacers { get => _gameObjectPlacers; }

            private List<IGameObjectPlacer> _gameObjectPlacers = new List<IGameObjectPlacer>();

            public void Enter()
            {
                // Loads all game objects.
                // Turn on transitioners.

                throw new System.NotImplementedException();
            }

            public void Exit()
            {
                // Unloads all game objects.
                // Turn off transitioners.

                throw new System.NotImplementedException();
            }

            // Start is called before the first frame update
            void Start()
            {
                GetBoundaries();
                GetGameObjectPlacers();
            }

            private void GetGameObjectPlacers()
            {
                foreach(ISectionTransitionController placer in _gameObjectPlacersContainer.GetComponentsInChildren<ISectionTransitionController>())
                {
                    _sectionTransitionControllers.Add(placer);
                }
            }

            private void GetBoundaries()
            {
                _sectionBoundaries = new SectionBoundaries();
                _sectionBoundaries.Height = TransformHeightToFloatValue(_cellsHeight);
                _sectionBoundaries.Width = TransformWidthToFloatValue(_cellsWidth);
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



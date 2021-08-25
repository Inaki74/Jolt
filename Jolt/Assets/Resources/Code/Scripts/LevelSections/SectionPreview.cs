using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace LevelSections
    {
        public class SectionPreview : MonoBehaviour
        {
            private float _gizmosHeight;
            private float _gizmosWidth;
            [SerializeField] private Section _mySection;

            private void OnDrawGizmos()
            {
                _gizmosHeight = _mySection.TransformHeightToFloatValue(_mySection.CellsHeight);
                _gizmosWidth = _mySection.TransformWidthToFloatValue(_mySection.CellsWidth);

                Vector3 center = new Vector3(transform.position.x - (_mySection.CellsWidth % 2 == 0 ? 0 : 1) * Section.CELLS_UNIT_RATIO / 2, transform.position.y - (_mySection.CellsHeight % 2 == 0 ? 0 : 1) * Section.CELLS_UNIT_RATIO / 2, 0f);
                Vector3 size = new Vector3(_gizmosWidth, _gizmosHeight, 0f);

                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(center, size);
            }
        }
    }
}



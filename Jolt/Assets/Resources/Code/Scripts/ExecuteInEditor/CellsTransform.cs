using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace ExecuteInEditor
    {
        using LevelSections;

        [ExecuteInEditMode]
        public class CellsTransform : MonoBehaviour
        {
            [SerializeField] private int _x;
            [SerializeField] private int _y;

            // Update is called once per frame
            void Update()
            {
                if(transform.position.x != _x * Section.CELLS_UNIT_RATIO)
                {
                    transform.position = new Vector3(_x * Section.CELLS_UNIT_RATIO, transform.position.y, 0f);
                }

                if (transform.position.y != _y * Section.CELLS_UNIT_RATIO)
                {
                    transform.position = new Vector3(transform.position.x, _y * Section.CELLS_UNIT_RATIO, 0f);
                }
            }
        }

    }
}


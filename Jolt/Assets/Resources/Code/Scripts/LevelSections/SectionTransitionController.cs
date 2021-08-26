using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace LevelSections
    {
        public class SectionTransitionController : ISectionTransitionController
        {
            [SerializeField] private Transform _raycastBeginTransform;
            [SerializeField] private float _raycastDistance;
            [SerializeField] private Vector2 _raycastDirection;
            [SerializeField] private LayerMask _whatIsPlayer;

            [SerializeField] private string _from;
            [SerializeField] private string _to;
            [SerializeField] private Transform _respawnTransform;

            public override string FromID { get => _from; }
            public override string ToID { get => _to; }

            public override Transform RespawnTransform => _respawnTransform;

            private void Start()
            {
                if(_raycastDirection == Vector2.zero)
                {
                    _raycastDirection = new Vector2(0f, -1f);
                }
            }

            protected override bool DetectPlayer()
            {
                // Detect player via raycast.
                Vector2 raycastBeginPosition = _raycastBeginTransform.position;

                var hit = Physics2D.Raycast(raycastBeginPosition, _raycastDirection, _raycastDistance, _whatIsPlayer);

                if (hit)
                {
                    Debug.DrawRay(raycastBeginPosition, _raycastDirection * hit.distance, Color.red, 2f);
                }
                else
                {
                    Debug.DrawRay(raycastBeginPosition, _raycastDirection * _raycastDistance, Color.green);
                }

                return hit;
            }
        }
    }
}



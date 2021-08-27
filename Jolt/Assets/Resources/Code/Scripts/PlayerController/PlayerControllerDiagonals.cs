using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Jolt
{
    namespace PlayerController
    {
        [RequireComponent(typeof(BoxCollider2D))]
        public class PlayerControllerDiagonals : MonoBehaviour//, IPlayerController
        {
            private Collider2D _collider;

            private const float _skinWidth = 0.025f;

            [SerializeField] private LayerMask _whatIsGround;

            private RaycastPositions _raycastPositions;

            private Vector2[] _topRaycastPositions;
            private Vector2[] _rightRaycastPositions;
            private Vector2[] _bottomRaycastPositions;
            private Vector2[] _leftRaycastPositions;

            [SerializeField] private int _horizontalRayCount;
            [SerializeField] private int _verticalRayCount;
            private float _horizontalRaySpacing;
            private float _verticalRaySpacing;

            // Start is called before the first frame update
            void Start()
            {
                GetComponents();
                CalculateRaySpacing();
            }

            void Update()
            {
                //CheckIfInsideCollider();
            }

            private void GetComponents()
            {
                _collider = GetComponent<BoxCollider2D>();
            }

            private void SetRaycastPositions()
            {
                Bounds colliderBounds = _collider.bounds;
                colliderBounds.Expand(-_skinWidth);

                _raycastPositions.topRight = new Vector2(colliderBounds.max.x, colliderBounds.max.y);
                _raycastPositions.topLeft = new Vector2(colliderBounds.min.x, colliderBounds.max.y);
                _raycastPositions.bottomRight = new Vector2(colliderBounds.max.x, colliderBounds.min.y);
                _raycastPositions.bottomLeft = new Vector2(colliderBounds.min.x, colliderBounds.min.y);
                _raycastPositions.center = colliderBounds.center;

                CalculateRaySpacing();

                SetRaycastPositionsArray(ref _topRaycastPositions, _horizontalRayCount, _raycastPositions.topLeft, Vector2.right, _horizontalRaySpacing);
                SetRaycastPositionsArray(ref _bottomRaycastPositions, _horizontalRayCount, _raycastPositions.bottomLeft, Vector2.right, _horizontalRaySpacing);
                SetRaycastPositionsArray(ref _rightRaycastPositions, _verticalRayCount, _raycastPositions.bottomRight, Vector2.up, _verticalRaySpacing);
                SetRaycastPositionsArray(ref _leftRaycastPositions, _verticalRayCount, _raycastPositions.bottomLeft, Vector2.up, _verticalRaySpacing);
            }

            private void SetRaycastPositionsArray(ref Vector2[] array, int count, Vector2 startPosition, Vector2 direction, float spacing)
            {
                array = new Vector2[count];

                array[0] = startPosition;
                // Top left -> top right
                for (int i = 1; i < count; i++)
                {
                    array[i] = startPosition + i * direction * spacing;
                }
            }

            private void CalculateRaySpacing()
            {
                Bounds colliderBounds = _collider.bounds;
                colliderBounds.Expand(-_skinWidth);

                _horizontalRayCount = Mathf.Clamp(_horizontalRayCount, 2, int.MaxValue);
                _verticalRayCount = Mathf.Clamp(_horizontalRayCount, 2, int.MaxValue);

                _horizontalRaySpacing = colliderBounds.size.x / (_horizontalRayCount - 1);
                _verticalRaySpacing = colliderBounds.size.y / (_verticalRayCount - 1);
            }

            public void Move(Vector2 direction)
            {
                Vector2 finalTranslation = new Vector2(direction.x * Time.deltaTime, direction.y * Time.deltaTime);

                SetRaycastPositions();

                finalTranslation = FindFinalTranslation(finalTranslation);

                transform.Translate(finalTranslation);
            }


            private Vector2 FindFinalTranslation(Vector2 translation)
            {
                // Find all collisions
                Vector2[] collisions = GetCollisions(translation);
                Vector2 smallest = translation;

                foreach (Vector2 collision in collisions)
                {
                    if (collision.magnitude < smallest.magnitude)
                    {
                        smallest = collision;
                    }
                }

                return smallest;
            }

            private Vector2[] GetCollisions(Vector2 translation)
            {
                List<Vector2> collisions = new List<Vector2>();

                if (translation.x > 0f)
                {
                    CheckCollisions(ref collisions, _rightRaycastPositions, translation);
                }
                else if (translation.x < 0f)
                {
                    CheckCollisions(ref collisions, _leftRaycastPositions, translation);
                }

                if (translation.y > 0f)
                {
                    CheckCollisions(ref collisions, _topRaycastPositions, translation);
                }
                else if (translation.y < 0f)
                {
                    CheckCollisions(ref collisions, _bottomRaycastPositions, translation);
                }

                return collisions.ToArray();
            }

            private void CheckCollisions(ref List<Vector2> addVectorsHere, Vector2[] raycastPositions, Vector2 translation)
            {
                Vector2 normalizedTranslation = translation.normalized;

                foreach (Vector2 position in raycastPositions)
                {
                    RaycastHit2D rayHit = Physics2D.Raycast(position, normalizedTranslation, translation.magnitude, _whatIsGround);

                    if (rayHit)
                    {
                        Vector2 normal = rayHit.normal;

                        if (Mathf.Abs(normal.x) > 0.05f && Mathf.Abs(normal.y) < 0.05f)
                        {
                            normalizedTranslation = new Vector2(normalizedTranslation.x * (rayHit.distance - _skinWidth), translation.y);
                            Debug.DrawRay(position, normalizedTranslation, Color.green);
                            addVectorsHere.Add(normalizedTranslation);
                            continue;
                        }
                        else if (Mathf.Abs(normal.y) > 0f)
                        {
                            normalizedTranslation = new Vector2(translation.x, normalizedTranslation.y * (rayHit.distance - _skinWidth));
                            Debug.DrawRay(position, normalizedTranslation, Color.green);
                            addVectorsHere.Add(normalizedTranslation);
                            continue;
                        }
                        else
                        {
                            Debug.DrawRay(position, normalizedTranslation * (rayHit.distance - _skinWidth), Color.green);
                            addVectorsHere.Add(normalizedTranslation * (rayHit.distance - _skinWidth));
                            continue;
                        }
                    }
                    else
                    {
                        Debug.DrawRay(position, translation, Color.blue);
                        addVectorsHere.Add(translation);
                    }
                }
            }
        }
    }
}

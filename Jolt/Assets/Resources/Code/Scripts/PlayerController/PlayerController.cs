using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Jolt
{
    namespace PlayerController
    {
        [RequireComponent(typeof(BoxCollider2D))]
        public class PlayerController : MonoBehaviour, IPlayerController
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

            private void SetRaycastPositionsArray(ref Vector2[] array, int count, Vector2 startPosition, Vector2 direction,  float spacing)
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

            private void CheckIfInsideCollider()
            {
                SetRaycastPositions();

                Vector2 startPositionLeft = _raycastPositions.bottomLeft;
                Vector2 startPositionTop = _raycastPositions.topLeft;
                Vector2 startPositionRight = _raycastPositions.bottomRight;
                Vector2 startPositionBottom = _raycastPositions.bottomLeft;
                float boundingBoxWidth = Mathf.Abs(_raycastPositions.bottomLeft.x - _raycastPositions.center.x);
                float boundingBoxHeight = Mathf.Abs(_raycastPositions.topLeft.y - _raycastPositions.center.y);

                for (int i = 0; i < _verticalRayCount; i++)
                {
                    RaycastHit2D rayHitLeftToRight = Physics2D.Raycast(startPositionLeft, Vector2.right, boundingBoxWidth, _whatIsGround);

                    if (rayHitLeftToRight)
                    {
                        Debug.DrawRay(startPositionLeft, Vector2.right * rayHitLeftToRight.distance, Color.green, 3f);
                        transform.Translate(rayHitLeftToRight.distance + _skinWidth, 0f, 0f);
                        break;
                    }

                    RaycastHit2D rayHitRightToLeft = Physics2D.Raycast(startPositionRight, Vector2.left, boundingBoxWidth, _whatIsGround);

                    if (rayHitRightToLeft)
                    {
                        //
                        Debug.DrawRay(startPositionLeft, Vector2.left * rayHitRightToLeft.distance, Color.green, 3f);
                        transform.Translate(-rayHitRightToLeft.distance + _skinWidth, 0f, 0f);
                        break;
                    }


                    Debug.DrawRay(startPositionLeft, Vector2.right * boundingBoxWidth, Color.blue);
                    Debug.DrawRay(startPositionRight, Vector2.left * boundingBoxWidth, Color.blue);
                    startPositionLeft += Vector2.up * _verticalRaySpacing;
                    startPositionRight += Vector2.up * _verticalRaySpacing;
                }

                for (int i = 0; i < _horizontalRayCount; i++)
                {
                    RaycastHit2D rayHitUpToDown = Physics2D.Raycast(startPositionTop, Vector2.down, boundingBoxHeight, _whatIsGround);

                    if (rayHitUpToDown)
                    {
                        //
                        Debug.DrawRay(startPositionTop, Vector2.down * rayHitUpToDown.distance, Color.green, 3f);
                        transform.Translate(0f, -rayHitUpToDown.distance + _skinWidth, 0f);
                        break;
                    }

                    RaycastHit2D rayHitDownToUp = Physics2D.Raycast(startPositionBottom, Vector2.up, boundingBoxHeight, _whatIsGround);

                    if (rayHitDownToUp)
                    {
                        Debug.DrawRay(startPositionBottom, Vector2.up * rayHitDownToUp.distance, Color.green, 3f);
                        transform.Translate(0f, rayHitDownToUp.distance + _skinWidth, 0f);
                        break;
                    }

                    Debug.DrawRay(startPositionTop, Vector2.down * boundingBoxHeight, Color.blue);
                    Debug.DrawRay(startPositionBottom, Vector2.up * boundingBoxHeight, Color.blue);
                    startPositionTop += Vector2.right * _horizontalRaySpacing;
                    startPositionBottom += Vector2.right * _horizontalRaySpacing;
                }
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

                foreach(Vector2 collision in collisions)
                {
                    if(collision.magnitude < smallest.magnitude)
                    {
                        smallest = collision;
                    }
                }

                return smallest;
            }

            private Vector2[] GetCollisions(Vector2 translation)
            {
                List<Vector2> collisions = new List<Vector2>();

                if(translation.x > 0f)
                {
                    CheckCollisions(ref collisions, _rightRaycastPositions, translation);
                }
                else if (translation.x < 0f)
                {
                    CheckCollisions(ref collisions, _leftRaycastPositions, translation);
                }

                if(translation.y > 0f)
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
                        Debug.DrawRay(position, normalizedTranslation * rayHit.distance, Color.green);
                        addVectorsHere.Add(normalizedTranslation * rayHit.distance);
                    }
                    else
                    {
                        Debug.DrawRay(position, translation, Color.blue);
                        addVectorsHere.Add(translation);
                    }
                }
            }

            private float CheckHorizontalCollisions(float distance)
            {
                Vector2 startPosition;
                float direction = Mathf.Sign(distance);
                float rayLength = Mathf.Abs(distance) + _skinWidth;

                if (direction > 0f)
                {
                    startPosition = _raycastPositions.bottomRight;
                }
                else
                {
                    startPosition = _raycastPositions.bottomLeft;
                }

                for (int i = 0; i < _verticalRayCount; i++)
                {
                    RaycastHit2D rayHit = Physics2D.Raycast(startPosition, Vector2.right * direction, rayLength, _whatIsGround);

                    if (rayHit)
                    {
                        Debug.DrawRay(startPosition, Vector2.right * direction * rayHit.distance, Color.green, 3f);
                        return rayHit.distance;
                    }

                    Debug.DrawRay(startPosition, Vector2.right * direction * rayLength, Color.blue);
                    startPosition += Vector2.up * _verticalRaySpacing;
                }

                return 0f;
            }

            private float CheckVerticalCollisions(float distance)
            {
                Vector2 startPosition;
                float direction = Mathf.Sign(distance);
                float rayLength = Mathf.Abs(distance) + _skinWidth;

                if (direction > 0f)
                {
                    startPosition = _raycastPositions.topLeft;
                }
                else
                {
                    startPosition = _raycastPositions.bottomLeft;
                }

                for (int i = 0; i < _horizontalRayCount; i++)
                {
                    RaycastHit2D rayHit = Physics2D.Raycast(startPosition, Vector2.up * direction, rayLength, _whatIsGround);

                    if (rayHit)
                    {
                        Debug.DrawRay(startPosition, Vector2.up * direction * rayHit.distance, Color.green, 3f);
                        return rayHit.distance;
                    }

                    Debug.DrawRay(startPosition, Vector2.up * direction * rayLength, Color.blue);
                    startPosition += Vector2.right * _horizontalRaySpacing;
                }

                return 0f;
            }
        }

        struct RaycastPositions
        {
            public Vector2 topLeft, topRight;
            public Vector2 bottomLeft, bottomRight;
            public Vector2 center;
        }
    }
}
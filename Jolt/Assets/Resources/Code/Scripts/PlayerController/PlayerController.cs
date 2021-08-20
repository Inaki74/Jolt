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
                CalculateRaySpacing();

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
                MoveX(Mathf.Sign(direction.x), Mathf.Abs(direction.x));
                MoveY(Mathf.Sign(direction.y), Mathf.Abs(direction.y));
            }

            public void MoveX(float direction, float speed)
            {
                SetRaycastPositions();

                CalculateRaySpacing();

                float hitDistance = CheckHorizontalCollisions(direction * speed * Time.deltaTime);

                float moveDistance;
                if(hitDistance == 0f)
                {
                    moveDistance = speed * Time.deltaTime;
                }
                else
                {
                    moveDistance = (hitDistance - _skinWidth);
                }

                transform.Translate(direction * moveDistance, 0f, 0f);
            }

            public void MoveY(float direction, float speed)
            {
                SetRaycastPositions();

                CalculateRaySpacing();

                float hitDistance = CheckVerticalCollisions(direction * speed * Time.deltaTime);

                float moveDistance;
                if (hitDistance == 0f)
                {
                    moveDistance = speed * Time.deltaTime;
                }
                else
                {
                    moveDistance = (hitDistance - _skinWidth);
                }

                transform.Translate(0f, direction * moveDistance, 0f);
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
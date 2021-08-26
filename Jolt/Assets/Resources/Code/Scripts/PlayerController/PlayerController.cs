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

            private bool _enteredColliderLastFrame;

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

            private bool CheckIfInsideCollider(float directionx, float directiony)
            {
                SetRaycastPositions();

                CalculateRaySpacing();

                Vector2 startPositionLeft = _raycastPositions.bottomLeft;
                Vector2 startPositionTop = _raycastPositions.topLeft;
                Vector2 startPositionRight = _raycastPositions.bottomRight;
                Vector2 startPositionBottom = _raycastPositions.bottomLeft;
                float boundingBoxWidth = Mathf.Abs(_raycastPositions.bottomLeft.x - _raycastPositions.bottomRight.x);
                float boundingBoxHeight = Mathf.Abs(_raycastPositions.topLeft.y - _raycastPositions.bottomLeft.y);

                for (int i = 0; i < _verticalRayCount; i++)
                {
                    if(directionx > 0f)
                    {
                        RaycastHit2D rayHitRightToLeft = Physics2D.Raycast(startPositionRight, Vector2.left, boundingBoxWidth, _whatIsGround);

                        if (rayHitRightToLeft)
                        {
                            Debug.DrawRay(startPositionLeft, Vector2.left * rayHitRightToLeft.distance, Color.green);
                            transform.Translate(-rayHitRightToLeft.distance - _skinWidth, 0f, 0f);
                            return true;
                        }

                        Debug.DrawRay(startPositionRight, Vector2.left * boundingBoxWidth, Color.cyan);
                        startPositionRight += Vector2.up * _verticalRaySpacing;
                    }
                    else if (directionx < 0f)
                    {
                        RaycastHit2D rayHitLeftToRight = Physics2D.Raycast(startPositionLeft, Vector2.right, boundingBoxWidth, _whatIsGround);

                        if (rayHitLeftToRight)
                        {
                            Debug.DrawRay(startPositionLeft, Vector2.right * rayHitLeftToRight.distance, Color.green);
                            transform.Translate(rayHitLeftToRight.distance + _skinWidth, 0f, 0f);
                            return true;
                        }

                        Debug.DrawRay(startPositionLeft, Vector2.right * boundingBoxWidth, Color.cyan);
                        startPositionLeft += Vector2.up * _verticalRaySpacing;
                    }
                }

                for (int i = 0; i < _horizontalRayCount; i++)
                {
                    if(directiony > 0f)
                    {
                        RaycastHit2D rayHitUpToDown = Physics2D.Raycast(startPositionTop, Vector2.down, boundingBoxHeight, _whatIsGround);

                        if (rayHitUpToDown)
                        {
                            //
                            Debug.DrawRay(startPositionTop, Vector2.down * rayHitUpToDown.distance, Color.green);
                            transform.Translate(0f, -rayHitUpToDown.distance - _skinWidth, 0f);
                            return true;
                        }

                        Debug.DrawRay(startPositionTop, Vector2.down * boundingBoxHeight, Color.cyan);
                        startPositionTop += Vector2.right * _horizontalRaySpacing;
                    }
                    else if (directiony < 0f)
                    {
                        RaycastHit2D rayHitDownToUp = Physics2D.Raycast(startPositionBottom, Vector2.up, boundingBoxHeight, _whatIsGround);

                        if (rayHitDownToUp)
                        {
                            Debug.DrawRay(startPositionBottom, Vector2.up * rayHitDownToUp.distance, Color.green);
                            transform.Translate(0f, rayHitDownToUp.distance + _skinWidth, 0f);
                            return true;
                        }

                        Debug.DrawRay(startPositionBottom, Vector2.up * boundingBoxHeight, Color.cyan);
                        startPositionBottom += Vector2.right * _horizontalRaySpacing;
                    }
                }

                return false;
            }

            public void Move(Vector2 direction)
            {
                if(direction != Vector2.zero)
                {
                    _enteredColliderLastFrame = CheckIfInsideCollider(direction.x, direction.y);

                    if (!_enteredColliderLastFrame)
                    {
                        MoveX(Mathf.Sign(direction.x), Mathf.Abs(direction.x));
                        MoveY(Mathf.Sign(direction.y), Mathf.Abs(direction.y));
                    }
                }
            }

            private void MoveX(float direction, float speed)
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

            private void MoveY(float direction, float speed)
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
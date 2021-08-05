using UnityEngine;

namespace Jolt.PlayerController
{
    public interface IPlayer
    {
        IPlayerStateMachine StateMachine { get; }
        IPlayerInputManager InputManager { get; }
        SpriteRenderer Sr { get; }
        CircleCollider2D Cc { get; }
        bool IsDead { get; set; }

        bool CheckHasReachedPoint(Vector2 point);
        bool CheckIsGrounded();
        bool CheckIsFreeFalling();
        bool CheckIsTouchingWallLeft();
        bool CheckIsTouchingWallRight();
        bool CheckIsTouchingNode();
        bool CheckIsTouchingRail();
        void DeactivateArrowRendering();
        Vector2 GetCurrentVelocity();
        Collider2D GetNodeInfo();
        RailController GetRailInfo();
        void InstantiateDeathParticles();
        void MoveTowardsVector(Vector2 vector, float velocity);
        void ResetPosition();
        void SetActivePhysicsCollider(bool set);
        void SetActiveSpriteRenderer(bool set);
        void SetArrowRendering();
        void SetGravityScale(float gravity);
        void SetDrag(float drag);
        void SetPosition(Vector2 position);
        void SetScale(Vector2 scale);
        void Dash(Vector3 startPos, Vector3 finalPos, float velocity);
        void MoveX(float direction, float velocity);
        void MoveY(float direction, float velocity);
    }
}
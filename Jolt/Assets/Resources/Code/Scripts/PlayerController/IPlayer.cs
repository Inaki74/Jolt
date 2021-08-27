using UnityEngine;

namespace Jolt.PlayerController
{
    public interface IPlayer
    {
        IPlayerStateMachine StateMachine { get; }
        IPlayerInputManager InputManager { get; }
        SpriteRenderer Sr { get; }
        BoxCollider2D Bc { get; }
        bool IsDead { get; set; }
        bool WallFlipped { get; set; }
        bool IsFacingRight { get; set; }
        Vector2 Velocity { get; set; }

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
        void ResetPosition();
        void SetActivePhysicsCollider(bool set);
        void SetDashCollider(bool set);
        void SetActiveSpriteRenderer(bool set);
        void SetArrowRendering();
        void SetGravityScale(float gravity);
        void SetMaxFallSpeed(float newFallSpeed);
        void SetPosition(Vector2 position);
        void SetScale(Vector2 scale);
        void Gravity();
        void ResetGravity();
        void SetVelocity(Vector2 velocity);
        void SetDashVectors(Vector3 startPos, Vector3 finalPos);
        void Dash(float distance, float velocity);
        void Move(Vector2 vector);
        void CheckIfShouldFlip(float direction);
        void FlipRight();
        void FlipLeft();
        void SetAnimationInt(string name, int value);
        void SetAnimationBool(string name, bool value);
        bool GetAnimationBool(string name);
        void Flip();
        void FlipY();
        void ActivateInput(bool set);
        void ResetJumpInputTimer();
    }
}
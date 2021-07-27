﻿using UnityEngine;

namespace Jolt.PlayerController
{
    public interface IPlayer
    {
        PlayerStateMachine StateMachine { get; }
        IPlayerInputManager InputManager { get; }
        Rigidbody2D Rb { get; }
        SpriteRenderer Sr { get; }
        CircleCollider2D Cc { get; }
        bool IsDead { get; set; }

        bool CheckHasReachedPoint(Vector2 point);
        bool CheckIsGrounded();
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
        void SetDashMovement(float velocity);
        void SetDashVectors(Vector3 startPos, Vector3 finalPos);
        void SetGravityScale(float gravity);
        void SetMovementByImpulse(Vector2 direction, float speed);
        void SetMovementXByForce(Vector2 direction, float speed);
        void SetMovementYByForce(Vector2 direction, float speed);
        void SetPosition(Vector2 position);
        void SetRigidbodyVelocityX(float velocity);
        void SetRigidbodyVelocityY(float velocity);
    }
}
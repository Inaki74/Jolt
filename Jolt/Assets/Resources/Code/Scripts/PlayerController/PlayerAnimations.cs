using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        [RequireComponent(typeof(SpriteRenderer))]
        public class PlayerAnimations : MonoBehaviour
        {
            private SpriteRenderer _sr;
            private Animator _animator;

            // Start is called before the first frame update
            void Start()
            {
                GetComponents();
            }

            private void GetComponents()
            {
                _sr = GetComponent<SpriteRenderer>();
                _animator = GetComponent<Animator>();
            }

            public void Flip()
            {
                _sr.flipX = !_sr.flipX;
            }

            public void SetAnimationBool(string name, bool value)
            {
                _animator.SetBool(name, value);
            }

            public static class Constants
            {
                public const string GROUNDED_BOOL = "isGrounded";
                public const string MOVE_BOOL = "isMoving";
                public const string DUCK_BOOL = "isDucking";
            }
        }
    }
}
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

            public void FlipY()
            {
                _sr.flipY = !_sr.flipY;
            }

            public void SetAnimationInt(string name, int value)
            {
                _animator.SetInteger(name, value);
            }

            public void SetAnimationBool(string name, bool value)
            {
                _animator.SetBool(name, value);
            }

            public bool GetAnimationBool(string name)
            {
                return _animator.GetBool(name);
            }

            public static class Constants
            {
                public const string IDLE_BOOL = "idle";
                public const string DUCK_BOOL = "duck";
                public const string LOOKUP_BOOL = "lookup";
                public const string WALK_BOOL = "walk";
                public const string FALL_BOOL = "fall";
                public const string RISE_BOOL = "rise";
                public const string WALLFALL_BOOL = "wallfall";
                public const string WALLRISE_BOOL = "wallrise";
                public const string WALLJUMP_BOOL = "walljump";
                public const string DASH_BOOL = "dash";
                public const string DASHX_INT = "dashX";
                public const string DASHY_INT = "dashY";
            }
        }
    }
}
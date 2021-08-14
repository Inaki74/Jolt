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

            // Start is called before the first frame update
            void Start()
            {
                GetComponents();
            }

            private void GetComponents()
            {
                _sr = GetComponent<SpriteRenderer>();
            }

            public void Flip()
            {
                _sr.flipX = !_sr.flipX;
            }
        }
    }
}
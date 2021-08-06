using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        public class Follow : MonoBehaviour
        {
            [SerializeField] private Transform _who;

            // Update is called once per frame
            void Update()
            {
                transform.position = _who.transform.position;
            }
        }
    }
}



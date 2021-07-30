using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        [CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Physics Data")]
        public class PlayerPhysicsData : ScriptableObject, IPlayerPhysicsData
        {
            [SerializeField] private float _standardGravity;

            [SerializeField] private float _standardLinearDrag;

            public float StandardGravity { get { return _standardGravity; } }
            public float StandardLinearDrag { get { return _standardLinearDrag; } }
        }
    }
}

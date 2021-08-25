using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

namespace Jolt
{
    namespace LevelSections
    {
        using PlayerController;

        public class SectionCameraAdjuster : MonoSingleton<SectionCameraAdjuster>, ISectionCameraAdjuster
        {
            [SerializeField] private CinemachineBrain _cinemachineBrain;

            public void Transition(ISection sectionToTransition, ISection sectionTransitionedFrom)
            {
                // Transitions camera to new section
                sectionTransitionedFrom.Camera.Follow = null;
                sectionTransitionedFrom.Camera.enabled = false;

                sectionToTransition.Camera.Follow = FindObjectOfType<Player>().transform; // assuming there is one and only one
                sectionToTransition.Camera.enabled = true;
            }

            // Start is called before the first frame update
            void Start()
            {

            }

            // Update is called once per frame
            void Update()
            {

            }
        }
    }
}



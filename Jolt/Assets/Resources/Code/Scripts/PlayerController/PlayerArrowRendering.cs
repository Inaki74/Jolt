using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        public class PlayerArrowRendering
        {
            public LineRenderer ArrowLr { get; private set; }

            public PlayerArrowRendering(LineRenderer lineRenderer)
            {
                ArrowLr = lineRenderer;

                ArrowLr.enabled = false;
                ArrowLr.startWidth = 0.3f; ArrowLr.endWidth = 0.001f;
            }

            public void RenderArrow(Vector2 start, Vector2 finish)
            {
                ArrowLr.enabled = true;
                ArrowLr.SetPosition(0, start);
                ArrowLr.SetPosition(1, finish);
            }

            public void DerenderArrow()
            {
                ArrowLr.SetPosition(0, Vector2.zero);
                ArrowLr.SetPosition(1, Vector2.zero);

                ArrowLr.enabled = false;
            }
        }
    }
}
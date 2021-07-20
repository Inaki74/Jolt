using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    public class CircleHelperFunctions : MonoBehaviour
    {
        private LineRenderer CircleLr;
        private int _positions = 199;
        // Start is called before the first frame update
        void Start()
        {
            CircleLr = GetComponent<LineRenderer>();
            CircleLr.enabled = false;
            CircleLr.startWidth = 0.03f; CircleLr.endWidth = 0.03f;
            CircleLr.material.color = Color.grey;
            CircleLr.positionCount = _positions + 1;
        }

        public void DerenderCircle()
        {
            CircleLr.enabled = false;
            Vector3[] aux = new Vector3[_positions + 1];
            for (int i = 0; i <= _positions; i += 1)
            {
                aux[i] = Vector3.zero;
            }
            CircleLr.SetPositions(aux);
        }

        public void DrawCircle(Vector2 center, float radius)
        {
            CircleLr.enabled = true;
            Vector2 aux = Vector2.zero;
            float x, y;
            Vector3[] auxAr = new Vector3[_positions + 1];
            for (int i = 0; i <= _positions; i += 1)
            {
                x = radius * Mathf.Cos(Mathf.Rad2Deg * i) + center.x;
                y = radius * Mathf.Sin(Mathf.Rad2Deg * i) + center.y;

                aux.Set(x, y);

                auxAr[i] = aux;

            }
            CircleLr.SetPositions(auxAr);
        }
    }
}



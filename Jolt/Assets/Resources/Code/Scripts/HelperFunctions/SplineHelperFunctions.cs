using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    public class SplineHelperFunctions
    {
        public static Vector3 SplineCurve(int n, int i, float t, Transform[] ControlPoints)
        {
            if (i > n)
            {
                return Vector3.zero;
            }
            else
            {
                return Comb(n, i) * ControlPoints[i].position * Mathf.Pow(1 - t, n - i) * Mathf.Pow(t, i) + SplineCurve(n, i + 1, t, ControlPoints);
            }
        }

        public static int Comb(int n, int i)
        {
            return Factorial(n) / (Factorial(n - i) * Factorial(i));
        }

        public static int Factorial(int n)
        {
            if (n <= 0)
            {
                return 1;
            }
            else
            {
                return n * Factorial(n - 1);
            }
        }
    }
}



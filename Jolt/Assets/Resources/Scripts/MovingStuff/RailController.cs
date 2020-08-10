using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailController : MonoBehaviour
{
    [SerializeField] private Transform[] ControlPoints;

    private LineRenderer Lr;

    private List<Vector3> LRPositions = new List<Vector3>();

    private Vector3 gizmosPositions;

    private void Start()
    {
        ControlPoints = InitializeControlPoints();
        Lr = GetComponent<LineRenderer>();

        Lr.startWidth = 0.1f; Lr.endWidth = 0.1f;

        RenderLine();
    }

    private void Update()
    {
        
    }

    private void RenderLine()
    {
        for(float t = 0; t <= 1; t += 0.02f)
        {
            LRPositions.Add(SplineHelperFunctions.SplineCurve(ControlPoints.Length - 1, 0, t, ControlPoints));
        }
        Lr.positionCount = LRPositions.Count;
        Lr.SetPositions(LRPositions.ToArray());
    }

    private Transform[] InitializeControlPoints()
    {
        List<Transform> aux = new List<Transform>();
        foreach(Transform t in transform)
        {
            aux.Add(t);
        }
        return aux.ToArray();
    }

    private void OnDrawGizmos()
    {
        ControlPoints = InitializeControlPoints();

        for (float t = 0; t <= 1; t += 0.02f)
        {
            gizmosPositions = SplineHelperFunctions.SplineCurve(ControlPoints.Length - 1, 0, t, ControlPoints);

            Gizmos.color = Color.red;

            Gizmos.DrawSphere(gizmosPositions, 0.1f);
        }

    }
}

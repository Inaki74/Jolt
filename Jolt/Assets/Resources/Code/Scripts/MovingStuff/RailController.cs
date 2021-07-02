using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailController : MonoBehaviour
{
    public Transform[] ControlPoints;

    public bool Inverted { get; private set; }

    private LineRenderer Lr;

    public List<Vector3> LRPositions { get; private set; }

    public float railSpeed { get; private set; }

    private int CPL;

    private float bronzeSpeed = 15f;
    private float silverSpeed = 25f;
    private float goldSpeed = 35f;

    public string railType; //e.g: Gold, bronze, Silver

    private Vector3 gizmosPositions;
    private Transform[] GizmoControlPoints;
    private int GCPL;

    private void Start()
    {
        ControlPoints = InitializeControlPoints();
        Inverted = false;
        CPL = ControlPoints.Length;
        LRPositions = new List<Vector3>();
        Lr = GetComponent<LineRenderer>();

        Lr.startWidth = 0.1f; Lr.endWidth = 0.1f;

        DecideRailType();
        RenderLine();
    }

    private void DecideRailType()
    {
        switch (railType)
        {
            case "Bronze":
                railSpeed = bronzeSpeed;
                Lr.material.color = new Color(ToPercentage(205f, 255f), ToPercentage(127f, 255f), ToPercentage(50f, 255f), 1f); // Bronze color
                break;
            case "Silver":
                railSpeed = silverSpeed;
                Lr.material.color = new Color(ToPercentage(192f, 255f), ToPercentage(192f, 255f), ToPercentage(192f, 255f), 1f); // Silver Color
                break;
            case "Gold":
                railSpeed = goldSpeed;
                Lr.material.color = new Color(ToPercentage(212f, 255f), ToPercentage(175f, 255f), ToPercentage(55f, 255f), 1f); // Silver Color
                break;
        }
    }

    private float ToPercentage(float amount, float max)
    {
        return amount * 100 / max / 100;
    }

    public void InvertControlPoints()
    {
        Transform[] aux = new Transform[CPL];

        for(int i = 0; i < CPL; i++)
        {;
            aux[i] = ControlPoints[(CPL - 1) - i];
        }

        ControlPoints = aux;
        Inverted = !Inverted;
    }

    private void RenderLine()
    {
        for(float t = 0; t <= 1; t += 0.02f)
        {
            LRPositions.Add(SplineHelperFunctions.SplineCurve(CPL - 1, 0, t, ControlPoints));
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
        GizmoControlPoints = InitializeControlPoints();
        GCPL = GizmoControlPoints.Length;

        for (float t = 0; t <= 1; t += 0.01f)
        {
            gizmosPositions = SplineHelperFunctions.SplineCurve(GCPL - 1, 0, t, GizmoControlPoints);

            Gizmos.color = Color.red;

            Gizmos.DrawSphere(gizmosPositions, 0.1f);
        }

    }
}

using System.Collections.Generic;
using UnityEngine;

public class HelixGizmo : MonoBehaviour
{
    [SerializeField] private Color _color1, _color2;
    [SerializeField]
    private bool _isTorus = false,
                 _drawControlPoints = false;
    [SerializeField] 
    private float _height = 5f,
                  _radius = 2f;
    [SerializeField]
    private int _turns = 1,
                _segmentCount = 50;
    [SerializeField] private float _controlPointsGizmoRadius = 1f;

    private void OnDrawGizmos()
    {
        Vector3[] controlPoints = CalculateControlPoints(_radius);
        List<Vector3> bezierPoints = GetBezierPoints(controlPoints);
        if (_drawControlPoints)
            DrawControlPoints(controlPoints);
        DrawWithColorBlend(bezierPoints, _color1, _color2);
    }

    private Vector3[] CalculateControlPoints(float radius)
    {
        Vector3[] unitSemiCirclePoints = new Vector3[4] { Vector3.right,
                                                          new Vector3(1, 4f/3f, 0), 
                                                          new Vector3(-1, 4f/3f, 0), 
                                                          new Vector3(-1, 0, 0) 
        };
        Vector3[] controlPoints = new Vector3[_turns * 8];
        Vector3 direction = new Vector3(1, 1, 1);
        float heightDelta = _height / (_turns * 6);
        int pointCount = 0;
      
        Vector3 normal = Vector3.zero;

        for (int i = 0; i < controlPoints.Length; i++)
        {
            if (pointCount % 4 == 0 && i != 0)
            {

                direction.x *= -1;
                direction.y *= -1;

                // Одна точка лишняя, поэтому скрываю её.
                normal.z -= heightDelta;
                pointCount = 0;
            }

            Vector3 uPoint = unitSemiCirclePoints[i % 4];
            var directedPoint = new Vector3(uPoint.x * direction.x, uPoint.y * direction.y, uPoint.z);

            if (_isTorus)
                normal.x += heightDelta;

            controlPoints[i] = (directedPoint * radius) + normal;

            normal.z += heightDelta;
            pointCount++;

        }

        return controlPoints;
    }

    private void Draw(List<Vector3> points)
    {
        for (int i = 0; i < points.Count - 1; i++)
            Gizmos.DrawLine(points[i], points[i + 1]);
    }

    private void DrawWithColorBlend(List<Vector3> points, Color color1, Color color2)
    {
        int len = points.Count;
        for (int i = 0; i < len - 1; i++)
        {
            float t = Mathf.InverseLerp(0, len, i);
            Gizmos.color = Color.Lerp(color1, color2, t);
            Gizmos.DrawLine(points[i], points[i + 1]);
        }
    }


    private void DrawControlPoints(Vector3[] points)
    {
        foreach (var p in points)
            Gizmos.DrawSphere(p, _controlPointsGizmoRadius);
    }

    public List<Vector3> GetBezierPoints(Vector3[] controlPoints)
    {
        int curveCount = ((controlPoints.Length / 4) - 1) * 4;
        List<Vector3> bezierPoints = new List<Vector3>();

        for (int j = 0; j < curveCount; j += 4)
            for (int i = 1; i <= _segmentCount; i++)
            {
                var t = i / (float)_segmentCount;
                Vector3 bezierPoint = CalculateCubicBezierPoint(t,
                                                                controlPoints[j],
                                                                controlPoints[j + 1],
                                                                controlPoints[j + 2],
                                                                controlPoints[j + 3]
                );
                bezierPoints.Add(bezierPoint);
            }

        return bezierPoints;
    }

    private Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        var u = 1 - t;
        var tt = t * t;
        var uu = u * u;
        var uuu = uu * u;
        var ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}

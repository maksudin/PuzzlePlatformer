using System.Collections.Generic;
using UnityEngine;

namespace Assets.PixelCrew.Utils.Draw
{
    public class DrawCubicBezier : MonoBehaviour
    {
        [SerializeField] private Transform[] _controlPoints;
        [SerializeField] private int _segmentCount = 50;

        private int _curveCount = 0;

        private void OnDrawGizmos()
        {
            _curveCount = _controlPoints.Length / 3;
            var bezierPoints = GetBezierPoints();
            if (bezierPoints.Count > 0)
                Draw(bezierPoints);
        }

        private void Draw(List<Vector3> points)
        {
            for (int i = 0; i < points.Count - 1; i++)
                Gizmos.DrawLine(points[i], points[i + 1]);
        }

        public List<Vector3> GetBezierPoints()
        {
            List<Vector3> _bezierPoints = new List<Vector3>();

            for (int j = 0; j < _curveCount; j++)
                for (int i = 1; i <= _segmentCount; i++)
                {
                    var t = i / (float)_segmentCount;
                    var nodeIndex = j * 3;
                    Vector3 bezierPoint =
                        CalculateCubicBezierPoint(t,
                                                 _controlPoints[nodeIndex].position,
                                                 _controlPoints[nodeIndex + 1].position,
                                                 _controlPoints[nodeIndex + 2].position,
                                                 _controlPoints[nodeIndex + 3].position
                    );
                    _bezierPoints.Add(bezierPoint);
                }

            return _bezierPoints;
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
}
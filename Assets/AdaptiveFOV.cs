using UnityEditor;
using UnityEngine;

public class AdaptiveFOV : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private FOVPoint[] _points;

    private void OnDrawGizmos()
    {
        var cameraPos = _camera.transform.position;
        var cameraDir = _camera.transform.forward;
        var angleDeg = 0f;
        foreach (var point in _points)
        {
            var pointPos = point.transform.position;
            var pointAngle = Vector3.Angle(cameraDir, pointPos - cameraPos);
            var r = point.Radius;
            var h = Vector3.Distance(cameraPos, pointPos);
            if (r > 0 && h > 0)
                pointAngle += Mathf.Asin(r / h) * Mathf.Rad2Deg;

            angleDeg = angleDeg < pointAngle? pointAngle : angleDeg;
            Handles.DrawWireDisc(pointPos, Vector3.forward, r);
        }

        _camera.fieldOfView = angleDeg * 2;
        Handles.DrawAAPolyLine(cameraPos, cameraDir);
    }
}
using UnityEditor;
using UnityEngine;

public class AdaptiveFOV : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private FOVPoint[] _points;

    private void OnDrawGizmos()
    {
        var cameraDir = _camera.transform.forward;
        var angleDeg = 0f;
        foreach (var point in _points)
        {
            var r = point.Radius;
            var h = Vector3.Distance(transform.position, point.transform.position - _camera.transform.position);
            var pointAngle = Vector3.Angle(cameraDir, point.transform.position - _camera.transform.position);
            if (r > 0 && h > 0)
                pointAngle += Mathf.Asin(r / h) * Mathf.Rad2Deg;

            angleDeg = angleDeg < pointAngle? pointAngle : angleDeg;
            Handles.DrawWireDisc(point.transform.position, Vector3.forward, r);
        }

        _camera.fieldOfView = angleDeg * 2;
        Handles.DrawAAPolyLine(_camera.transform.position, _camera.transform.forward);
    }
}
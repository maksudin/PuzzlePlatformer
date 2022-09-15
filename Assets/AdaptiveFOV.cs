using System;
using UnityEditor;
using UnityEngine;

public class AdaptiveFOV : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private FOVPoint[] _points;

    private void OnDrawGizmos()
    {
        var cameraPos = _cam.transform.position;
        var cameraDir = _cam.transform.forward;
        var cameraDirProjSpace = Vector2.right;
        var angleDeg = 0f;
        Handles.DrawAAPolyLine(cameraPos, cameraDir + cameraPos);

        foreach (var point in _points)
        {
            var pointPos = point.transform.position;
            var ptLocal = _cam.transform.InverseTransformPoint(pointPos);
            var ptProjSpace = new Vector2(ptLocal.z, ptLocal.y);
            var pointAngle = Vector3.Angle(cameraDirProjSpace, ptProjSpace);
            var r = point.Radius;
            var h = Vector3.Distance(cameraPos, pointPos);
            if (r > 0 && h > 0)
                pointAngle += Mathf.Asin(r / h) * Mathf.Rad2Deg;

            angleDeg = angleDeg < pointAngle? pointAngle : angleDeg;
            Gizmos.DrawWireSphere(pointPos,  r);
        }

        _cam.fieldOfView = angleDeg * 2;
    }
}
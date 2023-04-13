#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Assets.PixelCrew.Utils.Draw
{
    public class DrawHitOrigin : MonoBehaviour
    {
        void DrawRay(Vector3 p, Vector3 dir) => Handles.DrawAAPolyLine(p, p + dir);

        private void OnDrawGizmos()
        {
            Vector3 lookDir = transform.forward;
            Physics.Raycast(transform.position, lookDir, out RaycastHit hit);
            Debug.DrawRay(transform.position, lookDir, Color.white);

            if (hit.rigidbody != null)
            {
                var hitPos = hit.point;
                var up = hit.normal;
                var right = Vector3.Cross(up, lookDir).normalized;
                var forward = Vector3.Cross(right, up);

                Quaternion turretRot = Quaternion.LookRotation(forward, up);
                Matrix4x4 turretToWorld = Matrix4x4.TRS(hitPos, turretRot, Vector3.one);

                Vector3[] pts = new Vector3[]
                {
                new Vector3(1, 0, 1),
                new Vector3(-1, 0, 1),
                new Vector3(-1, 0, -1),
                new Vector3(1, 0, -1),
                new Vector3(1, 2, 1),
                new Vector3(-1, 2, 1),
                new Vector3(-1, 2, -1),
                new Vector3(1, 2, -1)
                };



                Handles.color = Color.green;
                DrawRay(hitPos, up);
                Handles.color = Color.cyan;
                DrawRay(hitPos, right);
                Handles.color = Color.red;
                DrawRay(hitPos, forward);

                foreach (var p in pts)
                {
                    Vector3 worldPt = turretToWorld.MultiplyPoint3x4(p);
                    Gizmos.DrawSphere(worldPt, 0.1f);
                }
            }
        }
    }
}
#endif

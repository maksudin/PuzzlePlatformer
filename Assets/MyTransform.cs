using UnityEngine;

public class MyTransform : MonoBehaviour
{
    [SerializeField] private Transform point;
    [SerializeField] private Vector2 coords;

    private void OnDrawGizmos()
    {
        Vector2 pointPos = point.position;
        Vector2 vectorX = transform.position + transform.right;
        Vector2 vectorY = transform.position + transform.up;

        var projX = Vector2.Dot(vectorX - pointPos, vectorX);
        var projY = Vector2.Dot(point.position, vectorY);
        coords = new Vector2(projX, projY);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, vectorX);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, vectorY);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, point.position);
    }
}

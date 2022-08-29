using UnityEngine;

public class MyTransform : MonoBehaviour
{
    [SerializeField] private Transform newTransform;
    [SerializeField] private Transform point;
    [SerializeField] private Vector2 coords;

    private void OnDrawGizmos()
    {
        Vector2 vectorP = point.position;
        Vector2 newTransformPos = newTransform.position;
        Vector2 vectorX = newTransformPos + (Vector2)newTransform.right;
        Vector2 vectorY = newTransformPos + (Vector2)newTransform.up;

        var projX = Vector2.Dot(vectorP - newTransformPos, vectorX - newTransformPos);
        var projY = Vector2.Dot(vectorP - newTransformPos, vectorY - newTransformPos);
        coords = new Vector2(projX, projY);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(newTransform.position, vectorX);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(newTransform.position, vectorY);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(newTransform.position, point.position);
    }
}

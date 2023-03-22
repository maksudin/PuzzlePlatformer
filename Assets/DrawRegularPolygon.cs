#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class DrawRegularPolygon : MonoBehaviour
{
    [SerializeField, Range(3, 20)] private int _sideCount = 5;
    [SerializeField, Range(1, 4)] private int density = 1;

    private void OnDrawGizmos()
    {
        var angleRad = 2 * Mathf.PI / _sideCount;
        Vector2[] vertices = new Vector2[_sideCount];

        for (int i = 0; i < _sideCount; i++)
        {
            var x = Mathf.Cos(angleRad * i);
            var y = Mathf.Sin(angleRad * i);

            vertices[i] = new Vector2(x, y);
        }

        for (int i = 0; i < _sideCount; i++)
            Handles.DrawLine(vertices[i], vertices[(i + density) % _sideCount]);
    }
}
#endif
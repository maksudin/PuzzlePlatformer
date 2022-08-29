using UnityEditor;
using UnityEngine;

public class DrawRegularPolygon : MonoBehaviour
{
    [Range(3, 20)]
    [SerializeField] private int _sideCount = 5;
    [Range(1, 4)]
    [SerializeField] private int density = 1;

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

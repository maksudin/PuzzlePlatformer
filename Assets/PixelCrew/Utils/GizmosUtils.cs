using UnityEngine;

namespace PixelCrew.Utils
{
    public class GizmosUtils
    {
        public static void DrawBounds(Bounds bounds, Color color)
        {
            var prevColor = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawLine(bounds.min, new Vector3(bounds.min.x, bounds.max.y)); // лево
            Gizmos.DrawLine(bounds.max, new Vector3(bounds.max.x, bounds.min.y)); // право
            Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.max.y), bounds.max); // верх
            Gizmos.DrawLine(new Vector3(bounds.max.x, bounds.min.y), bounds.min); // низ

            Gizmos.color = prevColor;
        }
    }
}
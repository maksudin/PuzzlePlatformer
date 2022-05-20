using UnityEngine;

namespace Assets.PixelCrew.Utils
{
    public class EasingUtils
    {
        public static float EaseInSine(float start, float end, float value)
        {
            end -= start;
            return -end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end + start;
        }
    }
}
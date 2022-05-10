using UnityEngine;

namespace PixelCrew.Utils
{
    public class WindowUtils
    {
        public static void CreateWindow(string path)
        {
            var window = Resources.Load<GameObject>(path);
            var canvas = GameObject.FindWithTag("HUDCanvas");
            Object.Instantiate(window, canvas.transform);
        }

    }
}
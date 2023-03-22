using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Assets.PixelCrew.Creatures.Mobs.Boss
{
    public class ModifyLights : MonoBehaviour
    {
        [SerializeField] private Light2D[] _lights;
        [SerializeField, ColorUsage(showAlpha: true, hdr: true)]
        private Color _color;

        [ContextMenu("Setup")]
        public void SetColor()
        {
            foreach (var light2d in _lights)
                light2d.color = _color;
        }
    }
}
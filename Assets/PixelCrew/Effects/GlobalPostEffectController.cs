using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.PixelCrew.Effects
{
    [RequireComponent(typeof(Volume))]
    public class GlobalPostEffectController : MonoBehaviour
    {
        private Volume _volume;

        private void Awake()
        {
            _volume = GetComponent<Volume>();
        }

        public void TurnOn()
        {
            _volume.enabled = true;
        }

        public void TurnOff()
        {
            _volume.enabled = false;
        }
    }
}
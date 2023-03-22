using PixelCrew.Components.ColliderBased;
using UnityEngine;

namespace PixelCrew.Components.Audio
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(LayerCheck))]
    public class PlaySoundOnCollisionComponent : MonoBehaviour
    {
        [SerializeField] private LayerCheck _layer;
        [SerializeField] private AudioSource[] _muteSources;

        private AudioSource _source;
        private void Awake() => _source = GetComponent<AudioSource>();

        private void Update()
        {
            if (_layer.IsTouchingLayer)
            {
                _source.mute = false;
                foreach (var mSource in _muteSources)
                    mSource.mute = true;
            }
            else
            {
                _source.mute = true;
                foreach (var mSource in _muteSources)
                    mSource.mute = false;
            }
        }
    }
}
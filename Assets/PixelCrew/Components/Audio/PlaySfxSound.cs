using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.Audio
{
    public class PlaySfxSound : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        private AudioSource _source;

        public void Play()
        {
            if (_source == null)
                _source = GameObject.FindGameObjectWithTag(AudioUtills.SfxSourceTag).GetComponent<AudioSource>();

            _source.PlayOneShot(_clip);
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDestroy;
        private AudioSource _audioSource;
        private bool _hasAudio = false;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _hasAudio = _audioSource? true : false;
        }

        public void DestroyObject()
        {
            if (_hasAudio)
            {
                Invoke(nameof(DestroyObjectWithAudio), _audioSource.clip.length);
            }
            else
            {
                Destroy(_objectToDestroy);
            }
        }

        private void DestroyObjectWithAudio()
        {
            Destroy(_objectToDestroy);
        }

    }

}

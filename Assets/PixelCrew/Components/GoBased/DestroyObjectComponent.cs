using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDestroy;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void DestroyObject()
        {
            if (_audioSource != null)
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

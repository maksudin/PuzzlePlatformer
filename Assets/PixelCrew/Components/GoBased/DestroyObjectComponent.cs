using Assets.PixelCrew.Components;
using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDestroy;
        [SerializeField] private RestoreStateComponent _state;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void DestroyObject()
        {
            if (_audioSource != null)
                Invoke(nameof(DestroyObjectWithAudio), _audioSource.clip.length);
            else
            {
                Destroy(_objectToDestroy);
                if (_state != null)
                    FindObjectOfType<GameSession>().StoreState(_state.Id);
            }
        }

        private void DestroyObjectWithAudio()
        {
            Destroy(_objectToDestroy);
        }

    }

}

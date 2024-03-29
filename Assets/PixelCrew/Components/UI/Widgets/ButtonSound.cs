﻿using PixelCrew.Components.Audio;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PixelCrew.Components.UI.Widgets
{
    public class ButtonSound : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private AudioClip _audioClip;
        private AudioSource _source;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_source == null)
                 _source = GameObject.FindWithTag(AudioUtills.SfxSourceTag).GetComponent<AudioSource>();

            _source.PlayOneShot(_audioClip);
        }
    }
}
﻿using System.Collections;
using Assets.PixelCrew.Components.UI.HUD.Dialogs;
using Assets.PixelCrew.Effects;
using PixelCrew.Model.Data;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.UI.HUD.Dialogs
{
    public class DialogBoxController : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private Animator _animator;

        [Space, SerializeField] private float _textSpeed = 0.09f;

        [Header("Sounds")]
        [SerializeField] private AudioClip _typing, _open, _close;
        [SerializeField] private DialogData _testData;
        [Space,SerializeField] protected DialogContent Content;
        
        private DialogData _data;
        private int _currentSentence;
        private GlobalPostEffectController _globalEffects;
        private AudioSource _sfxSource;
        private static readonly int IsOpen = Animator.StringToHash("is_open");
        private Coroutine _typingRoutine;
        protected float _defaultTimeScale;

        protected Sentence CurrentSentence => _data.Sentences[_currentSentence];
        private UnityEvent _onComplete;

        private void Start()
        {
            _defaultTimeScale = Time.timeScale;
            _globalEffects = FindObjectOfType<GlobalPostEffectController>();
            _sfxSource = AudioUtills.FindSfxSource();
        }

        protected virtual void OnStartDialogAnimation()
        {
            _typingRoutine = StartCoroutine(TypeDialogText());
            if (_globalEffects != null)
                _globalEffects.TurnOn();
        }

        protected virtual void OnCloseDialogAnimation()
        {
            if (_globalEffects != null)
                _globalEffects.TurnOff();
        }

        protected virtual DialogContent CurrentContent => Content;

        public void OnSkip()
        {
            if (_typingRoutine == null) return;

            StopTypeAnimation();
            CurrentContent.Text.text = _data.Sentences[_currentSentence].ValueId;
        }

        public void OnContinue()
        {
            StopTypeAnimation();
            _currentSentence++;

            var isDialogCompleted = _currentSentence >= _data.Sentences.Length;
            if (isDialogCompleted)
            {
                OnCloseDialogAnimation();
                HideDialogBox();
                _onComplete?.Invoke();
            }
            else
                OnStartDialogAnimation();
        }

        private void HideDialogBox()
        {
            _animator.SetBool(IsOpen, false);
            _sfxSource.PlayOneShot(_close);
        }

        private void StopTypeAnimation()
        {
            if (_typingRoutine != null)
                StopCoroutine(_typingRoutine);
            _typingRoutine = null;
        }

        private IEnumerator TypeDialogText()
        {
            CurrentContent.Text.text = string.Empty;
            var sentence = CurrentSentence;
            CurrentContent.TrySetIcon(sentence.Icon);
            foreach (var letter in sentence.ValueId)
            {
                CurrentContent.Text.text += letter;
                _sfxSource.PlayOneShot(_typing);
                yield return new WaitForSecondsRealtime(_textSpeed);
            }

            _typingRoutine = null;
        }

        public void ShowDialog(DialogData data, UnityEvent onComplete)
        {
            _onComplete = onComplete;
            _data = data;
            _currentSentence = 0;
            CurrentContent.Text.text = string.Empty;

            _container.SetActive(true);
            _sfxSource.PlayOneShot(_open);
            _animator.SetBool(IsOpen, true);
        }

        public void Pause() => Time.timeScale = 0;
        public void UnPause() => Time.timeScale = _defaultTimeScale;
    }
}
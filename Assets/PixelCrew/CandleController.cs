using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

namespace Assets.PixelCrew
{
    public class CandleController : MonoBehaviour
    {
        [SerializeField] private GameObject _candleObject;
        [SerializeField] private Light2D _light;
        [SerializeField] private Image _candleCooldownImage;
        [SerializeField] private float _candleCooldown = 20f;
        [SerializeField] UnityEvent OnRanOut;

        private float _defaultIntensity;
        private float _candleCapacity;
        private Coroutine _currentCoroutine;

        private void Awake()
        {
            _defaultIntensity = _light.intensity;
        }

        private void OnEnable()
        {
            _light.intensity = _defaultIntensity;

            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(UpdateIconCandleCooldown());
        }

        public void TurnOnCandle()
        {
            _candleObject.SetActive(true);
            _candleCooldownImage.gameObject.SetActive(true);
        }

        public void TurnOffCandle()
        {
            _candleObject.SetActive(false);
            _candleCooldownImage.gameObject.SetActive(false);
            OnRanOut?.Invoke();

            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
        }

        public void ResetCapacity()
        {
            _light.intensity = _defaultIntensity;
            _candleCapacity = 0f;
        }

        private IEnumerator UpdateIconCandleCooldown()
        {
            var next = 0f;
            var start = 1f;
            var delta = _light.intensity * 0.025f;

            while (_candleCapacity < 1f)
            {
                if (_candleCapacity >= 0.8f && _light.intensity > 0.1)
                    _light.intensity -= delta;

                _candleCapacity += Time.deltaTime / _candleCooldown;
                _candleCooldownImage.fillAmount = Mathf.Lerp(start, next, _candleCapacity);
                yield return null;
            }

            TurnOffCandle();
        }
    }
}
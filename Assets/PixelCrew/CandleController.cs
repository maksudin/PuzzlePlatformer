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
        [SerializeField] private float _candleCooldown = 10f;
        [SerializeField] UnityEvent OnRanOut;

        private float _defaultIntensity;
        private float _candleCapacity;
        private Coroutine _currentCoroutine;

        private void OnEnable()
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(UpdateIconCandleCooldown());
        }

        public void TurnOnCandle()
        {
            ResetCapacity();
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
            _candleCapacity = 0f;
        }

        private IEnumerator UpdateIconCandleCooldown()
        {
            var next = 0f;
            var start = 1f;

            while (_candleCapacity < 1f)
            {
                //if (_candleCapacity >= 0.8f && _light.intensity > 0) _light.intensity *= 0.1f;
                _candleCapacity += Time.deltaTime / _candleCooldown;
                _candleCooldownImage.fillAmount = Mathf.Lerp(start, next, _candleCapacity);
                yield return null;
            }

            TurnOffCandle();
        }
    }
}
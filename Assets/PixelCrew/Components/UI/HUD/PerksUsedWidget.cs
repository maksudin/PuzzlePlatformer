using System.Collections;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.PixelCrew.Components.UI.HUD
{
    public class PerksUsedWidget : MonoBehaviour
    {
        [SerializeField] private GameObject _iconObject;
        [SerializeField] private Image _perkCooldownImage;
        private GameSession _session;
        private float _perkCooldown;
        private CompositeDisposable _trash = new CompositeDisposable();
        private Coroutine _currentCoroutine;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _session.PerksModel.Subscribe(UpdateUsedPerk);
            _trash.Retain(_session.Data.Perks.UsedPerkCooldown.Subscribe(UpdatePerkCooldown));
        }

        private void OnDestroy()
        {
            _trash.Dispose();   
        }

        private void UpdateUsedPerk()
        {
            var perkId = _session.PerksModel.Used;
            if (perkId == "") return;

            var perkDef = DefsFacade.I.Perks.Get(perkId);
            _iconObject.GetComponent<Image>().sprite = perkDef.Icon;
            _session.Data.Perks.UsedPerkCooldown.Value = perkDef.Cooldown;
            _perkCooldown = perkDef.Cooldown;

            _iconObject.SetActive(true);
        }

        private void UpdatePerkCooldown()
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(UpdateIconPerkCooldown());
        }

        private IEnumerator UpdateIconPerkCooldown()
        {
            var next = 0f;
            var start = 1f;
            var t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime / _perkCooldown;
                _perkCooldownImage.fillAmount = Mathf.Lerp(start, next, t);
                yield return null;
            }

            StopCoroutine(_currentCoroutine);
        }
    }
}
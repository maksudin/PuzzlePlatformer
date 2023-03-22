using PixelCrew.Components.Health;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Components.UI.Widgets
{
    public class LifeBarWidget : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _lifeBar;
        [SerializeField] private HealthComponent _hp;
        private int _maxHp;
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            if (_hp == null)
                _hp = GetComponentInParent<HealthComponent>();

            _maxHp = _hp.MaxHealth;

            _trash.Retain(_hp.OnDie.Subscribe(OnDie));
            _trash.Retain(_hp.OnChange.Subscribe(OnHpChanged));

        }

        private void OnDestroy() => _trash.Dispose();
        private void OnDie() => Destroy(gameObject);

        private void OnHpChanged(int hp)
        {
            var progress = (float)hp / _maxHp;
            _lifeBar.SetProgress(progress);
        }
    }
}
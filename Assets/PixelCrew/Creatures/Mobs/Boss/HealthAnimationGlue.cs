using PixelCrew.Components.Health;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace Assets.PixelCrew.Creatures.Mobs.Boss
{
    public class HealthAnimationGlue : MonoBehaviour
    {
        private readonly int Health = Animator.StringToHash("health");
        [SerializeField] private HealthComponent _hp;
        [SerializeField] private Animator _animator;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Awake()
        {
            _trash.Retain(_hp.OnChange.Subscribe(OnHealthChanged));
            OnHealthChanged(_hp.CurrentHealth);
        }

        private void OnHealthChanged(int health)
        {
            _animator.SetInteger(Health, _hp.CurrentHealth);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}
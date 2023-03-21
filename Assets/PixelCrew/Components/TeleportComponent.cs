using Assets.PixelCrew.Creatures.Hero;
using PixelCrew.Creatures.Hero;
using UnityEngine;

namespace PixelCrew.Components
{
    [RequireComponent(typeof(InputEnableComponent))]
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destTransform;
        private static readonly int TeleportEnded = Animator.StringToHash("teleport_end");
        private static readonly int TeleportStarted = Animator.StringToHash("teleport_start");
        private Hero hero;
        private Animator _animator;
        private InputEnableComponent _input;

        public void TeleportHero()
        {
            hero = FindObjectOfType<Hero>();
            _input = FindObjectOfType<InputEnableComponent>();
            _input.SetInput(false);
            _animator = hero.GetComponent<Animator>();
            _animator.SetTrigger(TeleportStarted);

            hero.OnTeleportStartAnimEnded += Teleport;
            hero.OnTeleportEndAnimEnded += OnTeleportEnded;
        }

        private void Teleport()
        {
            hero.transform.position = _destTransform.position;
            _animator.SetTrigger(TeleportEnded);
        }

        private void OnTeleportEnded()
        {
            _input.SetInput(true);
        }

        private void OnDestroy()
        {
            // Получается что объект героя раньше уничтожился чем мы успели отписаться
            hero.OnTeleportStartAnimEnded -= Teleport;
            hero.OnTeleportEndAnimEnded -= Teleport;
        }
    }
}



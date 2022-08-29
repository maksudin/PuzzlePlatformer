using PixelCrew.Creatures.Hero;
using UnityEngine;

namespace PixelCrew.Components
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destTransform;
        
        private static readonly int TeleportEnded = Animator.StringToHash("teleport_end");
        private static readonly int TeleportStarted = Animator.StringToHash("teleport_start");
        private Hero hero;
        private Animator _animator;

        public void TeleportHero()
        {
            hero = FindObjectOfType<Hero>();
            _animator = hero.GetComponent<Animator>();
            _animator.SetTrigger(TeleportStarted);

            hero.OnTeleportAnimEnded += Teleport;
        }

        private void Teleport()
        {
            hero.transform.position = _destTransform.position;
            _animator.SetTrigger(TeleportEnded);
        }
    }
}



using System.Collections;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class TotemAi : MonoBehaviour
    {
        BaseShootingTrap[] _traps;
        [SerializeField] private float _cooldown;
        private Coroutine _current;

        private void Awake()
        {
            _traps = GetComponentsInChildren<BaseShootingTrap>();
            foreach (var trap in _traps)
            {
                trap.IsTotem = true;
            }
        }

        private void OnEnable()
        {
            RangeAttackLoop();
        }

        public void RangeAttackLoop()
        {
            if (_current != null)
                StopCoroutine(_current);

            _current = StartCoroutine(RangeCouroutine());
        }

        private IEnumerator RangeCouroutine()
        {
            while (enabled)
            {
                foreach (var trap in _traps)
                {
                    trap.RangeAttack();
                    yield return new WaitForSeconds(_cooldown);
                }

            }

            StopCoroutine(RangeCouroutine());
        }




    }
}
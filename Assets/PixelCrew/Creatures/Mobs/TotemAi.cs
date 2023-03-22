using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class TotemAi : MonoBehaviour
    {
        BaseShootingTrap[] _totemHeads;
        [SerializeField] private float _cooldown;
        private Coroutine _current;

        private void Awake()
        {
            _totemHeads = GetComponentsInChildren<BaseShootingTrap>();
            foreach (var head in _totemHeads)
                head.IsTotem = true;
        }

        private void OnEnable() => RangeAttackLoop();

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
                foreach (var head in _totemHeads)
                {
                    head.RangeAttack();
                    yield return new WaitForSeconds(_cooldown);
                }
            }

            StopCoroutine(RangeCouroutine());
        }
    }
}
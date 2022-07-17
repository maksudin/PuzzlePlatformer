using UnityEngine;
using Assets.PixelCrew.Model.Definitions.Player;
using PixelCrew.Model;

namespace PixelCrew.Components.Health
{
    public class HealthManagmentComponent : MonoBehaviour
    {
        [SerializeField] private HealthMode _mode;
        [SerializeField] private int _damage;
        [SerializeField] private StatId _statId;
        [SerializeField] private int _healValue;
        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }

        public void TakeDamage(GameObject target)
        {
            var health = target.GetComponent<HealthComponent>();
            switch (_mode)
            {
                case HealthMode.Bound:
                    health?.ApplyDamage(_damage);
                    break;
                case HealthMode.ExternalStat:
                    //if (_statId == StatId.CriticalDamage)
                    //{
                    //    health?.ApplyDamage(ModifyDamageByCrit((int)_session.StatsModel.GetValue(_statId)));
                    //    break;
                    //}
                    var statDamage = _session.StatsModel.GetValue(_statId);
                    health?.ApplyDamage((int)statDamage);
                    break;
            }
        }

        private int ModifyDamageByCrit(int damage)
        {
            var critChance = _session.StatsModel.GetValue(StatId.CriticalDamage);
            if (Random.value * 100 <= critChance)
                return damage * 2;
            return damage;
        }

        public void Heal(GameObject target)
        {
            var health = target.GetComponent<HealthComponent>();
            health?.Heal(_healValue);
        }
    }

    public enum HealthMode
    {
        Bound,
        ExternalStat
    }
}
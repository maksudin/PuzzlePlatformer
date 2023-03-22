using System;
using System.Collections;
using Assets.PixelCrew.Creatures.Weapons;
using PixelCrew.Utils;
using UnityEngine;

namespace Assets.PixelCrew.Components.GoBased
{
    public class CircularProjectileSpawner : MonoBehaviour
    {
        [SerializeField] CircularProjectileSettings[] _settings;
        public int Stage { get;  set; }

        [ContextMenu("Launch")]
        public void LaunchProjectiles() => StartCoroutine(SpawnProjectiles());

        private IEnumerator SpawnProjectiles()
        {
            var setting = _settings[Stage];
            var sectorStep = 2 * Mathf.PI / setting.BurstCount;

            for (int i = 0; i < setting.BurstCount; i++)
            {
                var angle = sectorStep * i;
                var direction = new Vector2(setting.Radius * Mathf.Cos(angle), setting.Radius *  Mathf.Sin(angle));
                var instance = SpawnUtills.Spawn(setting.Prefab.gameObject, transform.position);
                var projectile = instance.GetComponent<DirectionalProjectile>();
                projectile.Launch(direction);

                yield return new WaitForSeconds(setting.Delay);
            }
        }
    }

    [Serializable]
    public struct CircularProjectileSettings
    {
        [SerializeField] private DirectionalProjectile prefab;
        [SerializeField] private int _burstCount;
        [SerializeField] private float _delay;
        [SerializeField] private float _radius;

        public DirectionalProjectile Prefab => prefab;
        public int BurstCount => _burstCount;
        public float Delay => _delay;
        public float Radius => _radius;
    }
}
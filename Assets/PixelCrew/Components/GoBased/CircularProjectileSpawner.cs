﻿using System.Collections;
using Assets.PixelCrew.Creatures.Weapons;
using PixelCrew.Utils;
using UnityEngine;

namespace Assets.PixelCrew.Components.GoBased
{
    public class CircularProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private DirectionalProjectile _projectile;
        [SerializeField] private int _burstCount;
        [SerializeField] private float _delay;
        [SerializeField] private float _radius = 3f;

        [ContextMenu("Launch")]
        public void LaunchProjectiles()
        {
            StartCoroutine(SpawnProjectiles());
        }

        private IEnumerator SpawnProjectiles()
        {
            var sectorStep = 2 * Mathf.PI / _burstCount;

            for (int i = 0; i < _burstCount; i++)
            {
                var angle = sectorStep * i;
                var direction = new Vector2(_radius * Mathf.Cos(angle), _radius *  Mathf.Sin(angle));
                var instance = SpawnUtills.Spawn(_projectile.gameObject, transform.position);
                var projectile = instance.GetComponent<DirectionalProjectile>();
                projectile.Launch(direction);

                yield return new WaitForSeconds(_delay);
            }
        }
    }
}
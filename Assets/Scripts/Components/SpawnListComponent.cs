using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Components
{
    public class SpawnListComponent : MonoBehaviour
    {
        [SerializeField] private SpawnData[] _spawners;

        public void Spawn(string id)
        {
            var spawner = _spawners.FirstOrDefault(x => x.Id == id);
            spawner?.Component.Spawn();
        }

        public void SpawnWithOffset(string id, Vector2 offset)
        {
            var spawner = _spawners.FirstOrDefault(x => x.Id == id);
            spawner?.Component.SpawnWithOffset(offset);
        }

        [Serializable]
        public class SpawnData
        {
            public string Id;
            public SpawnComponent Component;
        }
    }
}


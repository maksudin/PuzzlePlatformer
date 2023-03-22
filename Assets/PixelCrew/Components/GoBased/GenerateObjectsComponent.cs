using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    [Serializable]
    public class ItemProbability
    {
        public GameObject Prefub;
        public float Probability;
    }

    [RequireComponent(typeof(SpawnComponent))]
    public class GenerateObjectsComponent : MonoBehaviour
    {
        [Range (0, 100)]
        [SerializeField] private int _itemAmount;
        [SerializeField] private List<ItemProbability> _itemProbabilityList;
        [SerializeField] private Collider2D _spawnCollider;
        private SpawnComponent _spawn;
        private Vector3 _spawnSizeMin;
        private Vector3 _spawnSizeMax;

        public void Awake() => _spawn = GetComponent<SpawnComponent>();

        public void GenerateCoins()
        {
            _spawnSizeMin = _spawnCollider.bounds.min;
            _spawnSizeMax = _spawnCollider.bounds.max;

            GameObject[] _generatedCoins = new GameObject[_itemAmount];

            // Сортирую вероятности от меньшего к большему. 
            _itemProbabilityList.Sort((a,b) => a.Probability.CompareTo(b.Probability));

            var bestChanceItem = _itemProbabilityList[_itemProbabilityList.Count - 1];

            float random;
            for (var i = 0; i < _itemAmount; i++)
            {
                random = UnityEngine.Random.value * bestChanceItem.Probability;

                if (random >= bestChanceItem.Probability)
                {
                    _generatedCoins[i] = bestChanceItem.Prefub;
                    break;
                }

                foreach (ItemProbability item in _itemProbabilityList)
                {
                    if (random < item.Probability)
                    {
                        _generatedCoins[i] = item.Prefub;
                        break;
                    }
                }
            }

            _spawn.SpawnOnRandomPositionRange(_spawnSizeMin, _spawnSizeMax, _generatedCoins);
        }
    }
}

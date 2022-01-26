using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            GameObject instantiate = Instantiate(_prefab, _target.position, Quaternion.identity);
            instantiate.transform.localScale = _target.lossyScale;
        }

        public void SpawnWithOffset(Vector2 offset)
        {
            Vector3 newPosition = new Vector3(_target.position.x + offset.x,
                                              _target.position.y + offset.y,
                                              _target.position.z);
            GameObject instantiate = Instantiate(_prefab, newPosition, Quaternion.identity);
            instantiate.transform.localScale = _target.lossyScale;
        }

        public void SpawnOnRandomPositionRange(Vector3 minPos, Vector3 maxPos, GameObject[] _prefubs)
        {

            foreach (GameObject go in _prefubs)
            {
                Vector3 newPosition = new Vector3(Random.Range(minPos.x, maxPos.x),
                                                  Random.Range(minPos.y, maxPos.y),
                                                  _target.position.z);
                GameObject instantiate = Instantiate(go, newPosition, Quaternion.identity); ;
                instantiate.transform.localScale = _target.lossyScale;
            }
            
        }
    }
}



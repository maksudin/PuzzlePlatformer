using System.Collections;
using UnityEngine;

namespace PixelCrew.Utils
{
    public class SpawnUtills
    {
        private const string ContainerName = "###SPAWNED###";

        public static GameObject Spawn(GameObject prefab, Vector3 position, string containerName = ContainerName)
        {
            var container = GameObject.Find(containerName);
            if (container == null)
                container = new GameObject(containerName);

            return GameObject.Instantiate(prefab, position, Quaternion.identity, container.transform);
        }

    }
}
using Cinemachine;
using PixelCrew.Creatures.Hero;
using UnityEngine;

namespace PixelCrew.Components.LevelManagement
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class SetFollowComponent : MonoBehaviour
    {
        private void Start()
        {
            var vCamera = GetComponent<CinemachineVirtualCamera>();
            var hero = FindObjectOfType<Hero>();
            if (hero == null) return;
            vCamera.Follow = hero.transform;
        }
    }
}
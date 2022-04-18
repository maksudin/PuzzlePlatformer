using System.Collections;
using System.Collections.Generic;
using PixelCrew.Creatures.Hero;
using UnityEngine;

namespace PixelCrew.Components
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destTransform;

        public void TeleportHero()
        {
            var hero = FindObjectOfType<Hero>();
            hero.transform.position = _destTransform.position;
        }
    }
}



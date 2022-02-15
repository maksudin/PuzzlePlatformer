using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PixelCrew.Creatures.Hero;

namespace PixelCrew.Components
{
    public class ArmHeroComponent : MonoBehaviour
    {

        public void ArmHero(GameObject go) 
        {
            Hero hero = go.GetComponent<Hero>();
            hero?.ArmHero();
        }
    }
}


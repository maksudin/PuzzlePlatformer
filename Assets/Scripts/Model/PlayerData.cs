using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Model
{
    [Serializable]
    public class PlayerData
    {
        public int Coins;
        public int Hp;
        public bool IsArmed;
        public Vector3 CheckPointPos;

        public PlayerData ShallowCopy()
        {
            return (PlayerData) this.MemberwiseClone();
        }
    }
}



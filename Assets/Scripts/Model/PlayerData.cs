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
        public Transform CheckPointPos;

        public PlayerData DeepCopy()
        {
            PlayerData other = (PlayerData)this.MemberwiseClone();
            other.Coins = Coins;
            other.Hp = Hp;
            other.IsArmed = IsArmed;
            other.CheckPointPos = CheckPointPos;
            return other;
        }
    }
}



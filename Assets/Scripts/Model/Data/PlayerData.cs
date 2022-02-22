using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Model.Data;
using UnityEngine;

namespace PixelCrew.Model
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory;

        //public int Coins;
        //public int Swords;
        public int Hp;
        //public bool IsArmed;

        public InventoryData Inventory => _inventory;

        public PlayerData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }
}



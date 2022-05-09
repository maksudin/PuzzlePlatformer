using System;
using Assets.PixelCrew.Model.Data;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using UnityEngine;

namespace PixelCrew.Model
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory;

        [Header("Params")]
        //public int Hp;
        public IntProperty Hp = new IntProperty();
        public PerksData Perks = new PerksData();
        public LevelData Levels = new LevelData();

        public InventoryData Inventory => _inventory;

        public PlayerData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }
}



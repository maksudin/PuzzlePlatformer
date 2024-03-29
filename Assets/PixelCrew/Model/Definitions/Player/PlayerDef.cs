﻿using UnityEngine;

namespace Assets.PixelCrew.Model.Definitions.Player
{
    [CreateAssetMenu(menuName = "Defs/PlayerDef", fileName = "PlayerDef")]
    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int _inventorySize;
        [SerializeField] private int _maxHealth;
        [SerializeField] private StatDef[] _stats;

        public int InventorySize => _inventorySize;
        public int MaxHealth => _maxHealth;
        public StatDef[] Stats => _stats;
        public StatDef GetStat(StatId id)
        {
            foreach (var stat in _stats)
                if (stat.ID == id)
                    return stat;

            return default;
        }
    }
}
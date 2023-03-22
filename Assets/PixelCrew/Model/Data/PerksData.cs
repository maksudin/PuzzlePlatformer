﻿using System;
using System.Collections.Generic;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class PerksData
    {
        [SerializeField] private StringProperty _used = new StringProperty();
        [SerializeField] private Cooldown _usedPerkCooldown;
        [SerializeField] private List<string> _unlocked;
        public StringProperty Used => _used;
        public Cooldown UsedPerkCooldown => _usedPerkCooldown;

        public void AddPerk(string id)
        {
            if (!_unlocked.Contains(id))
                _unlocked.Add(id);
        }

        public bool IsUnlocked(string id)
        {
            return _unlocked.Contains(id);
        }
    }
}
﻿using System;
using Assets.PixelCrew.Model.Definitions.Player;
using PixelCrew.Model;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;

namespace Assets.PixelCrew.Model.Data.Models
{
    public class StatsModel : IDisposable
    {
        private readonly PlayerData _data;

        public event Action OnChanged;
        public event Action<StatId> OnUpgraded;

        public ObservableProperty<StatId> InterfaceSelectedStat = new ObservableProperty<StatId>();

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        public StatsModel(PlayerData data)
        {
            _data = data;
            _trash.Retain(InterfaceSelectedStat.Subscribe((x, y) => OnChanged?.Invoke()));
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public void LevelUp(StatId id)
        {
            var def = DefsFacade.I.Player.GetStat(id);
            var nextLevel = GetCurrentLevel(id) + 1;

            if (def.Levels.Length <= nextLevel) return;
            
            var price = def.Levels[nextLevel].Price;
            if (!_data.Inventory.IsEnough(price)) return;

            _data.Inventory.Remove(price.ItemId, price.Count);
            _data.Levels.LevelUp(id);

            //PostProcessLevelUp(id);
            OnUpgraded?.Invoke(id);
            OnChanged?.Invoke();
        }

        //private void PostProcessLevelUp(StatId id)
        //{
        //    switch (id)
        //    {
        //        case StatId.Hp:
        //            _data.Hp.Value = (int)GetValue(id);
        //            break;
        //        case StatId.Speed:
        //            break;
        //        case StatId.RangeDamage:
        //            break;
        //    }
        //}

        public float GetValue(StatId id, int level = -1)
        {
            return GetLevelDef(id, level).Value;
        }

        public StatLevelDef GetLevelDef(StatId id, int level = -1)
        {
            if (level == -1) level = GetCurrentLevel(id);
            var def = DefsFacade.I.Player.GetStat(id);
            if (def.Levels.Length > level)
                return def.Levels[level];

            return default;
        }

        public int GetCurrentLevel(StatId id) => _data.Levels.GetLevel(id);

        public void Dispose()
        {
            _trash.Dispose();
        }
    }
}
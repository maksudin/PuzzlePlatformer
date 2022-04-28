using System;
using PixelCrew.Model;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;

namespace Assets.PixelCrew.Model.Data.Models
{
    public class PerksModel : IDisposable
    {
        private readonly PlayerData _data;
        public readonly StringProperty InterfaceSelection = new StringProperty();

        public PerksModel(PlayerData data)
        {
            _data = data;
        }

        public void Unlock(string id)
        {
            var def = DefsFacade.I.Perks.Get(id);
            var isEnoughResources = _data.Inventory.IsEnough(def.Price);

            if (isEnoughResources)
            {
                _data.Inventory.Remove(def.Price.ItemId, def.Price.Count);
                _data.Perks.AddPerk(id);
            }
        }

        public void UsePerk(string selected)
        {
            _data.Perks.Used.Value = selected;
        }

        public void Dispose()
        {
        }

        public bool IsUsed(string perkId)
        {
            return _data.Perks.Used.Value == perkId; 
        }

        public bool IsUnlocked(string perkId)
        {
            return _data.Perks.IsUnlocked(perkId);
        }
    }
}
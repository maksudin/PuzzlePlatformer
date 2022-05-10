using System;
using System.Collections.Generic;
using System.Linq;
using PixelCrew.Model.Definitions.Items;
using PixelCrew.Model.Definitions;
using UnityEngine;
using PixelCrew.Model.Definitions.Repository;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();

        public delegate void OnInventoryChanged(string id, int value);

        // Эквивалент делегату public Action<string, int> OnChanged;

        public OnInventoryChanged OnChangedInventory;
        public event Action OnChanged;

        public void Add(string id, int value)
        {
            if (value <= 0) return;

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;
            if (!itemDef.HasTag(ItemTag.Stackable))
            {
                var newItem = new InventoryItemData(id);
                _inventory.Add(newItem);
                newItem.Value += value;
                OnChangedInventory?.Invoke(id, Count(id));
                OnChanged?.Invoke();
                return;
            }

            var item = GetItem(id);
            if (item == null)
            {
                item = new InventoryItemData(id);
                _inventory.Add(item);
            }

            if (item.Value == itemDef.MaxAmount && itemDef.MaxAmount != 0) return;

            item.Value += value;
            OnChangedInventory?.Invoke(id, Count(id));
            OnChanged?.Invoke();

        }

        public void Remove(string id, int value)
        {
            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;
            
            //if (!itemDef.ShouldStack)
            //{
            //    for (var i = 0; i < value; i++)
            //    {
            //        var item = GetItem(id);
            //        if (item == null) return;
            //        _inventory.Remove(item);
            //    }
            //    return;
            //}


            var item = GetItem(id);
            if (item == null) return;

            item.Value -= value;

            if (item.Value <= 0)
                _inventory.Remove(item);

            OnChangedInventory?.Invoke(id, Count(id));
            OnChanged?.Invoke();

        }

        public bool IsEnough(params ItemWithCount[] items)
        {
            var joined = new Dictionary<string, int>();

            foreach (var item in items)
            {
                if (joined.ContainsKey(item.ItemId))
                    joined[item.ItemId] += item.Count;
                else
                    joined.Add(item.ItemId, item.Count);
            }

            foreach (var kvp in joined)
            {
                var count = Count(kvp.Key);
                if (count < kvp.Value) return false;
            }

            return true;
        }


        private InventoryItemData GetItem(string id)
        {
            foreach (var itemData in _inventory)
            {
                if (itemData.Id == id)
                    return itemData;
            }

            return null;
        }

        public int Count(string id)
        {
            var count = 0;
            foreach (var item in _inventory)
            {
                if (item.Id == id)
                    count += item.Value;
            }
            return count;
        }

        public InventoryItemData[] GetAll(params ItemTag[] tags)
        {
            var retValue = new List<InventoryItemData>();
            foreach (var item in _inventory)
            {
                var itemDef = DefsFacade.I.Items.Get(item.Id);
                var isAllRequirenmentMet = tags.All(x => itemDef.HasTag(x));
                if (isAllRequirenmentMet)
                    retValue.Add(item);
            }
            return retValue.ToArray();
        }
    }

    [Serializable]
    public class InventoryItemData
    {
        [InventoryId] public string Id;
        public int Value;

        public InventoryItemData(string id)
        {
            Id = id;
        }
    }
}
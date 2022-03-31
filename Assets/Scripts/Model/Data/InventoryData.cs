using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Model.Definitions;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();

        public delegate void OnInventoryChanged(string id, int value);
        // Эквивалент делегату public Action<string, int> OnChanged;

        public OnInventoryChanged OnChanged;

        public void Add(string id, int value)
        {
            if (value <= 0) return;

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;
            if (!itemDef.ShouldStack)
            {
                var newItem = new InventoryItemData(id);
                _inventory.Add(newItem);
                newItem.Value += value;
                OnChanged?.Invoke(id, Count(id));
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
            OnChanged?.Invoke(id, Count(id));
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

            OnChanged?.Invoke(id, Count(id));
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

        public InventoryItemData[] GetAll()
        {
            return _inventory.ToArray();
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
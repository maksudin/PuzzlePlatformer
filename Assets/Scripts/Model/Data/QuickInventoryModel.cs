using System;
using System.Collections;
using PixelCrew.Model.Data.Properties;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    public class QuickInventoryModel : MonoBehaviour
    {
        private PlayerData _data;

        public InventoryItemData[] Inventory { get; private set; }

        public readonly IntProperty SelectedIndex = new IntProperty();

        public QuickInventoryModel(PlayerData data)
        {
            _data = data;

            Inventory = _data.Inventory.GetAll();
            _data.Inventory.OnChanged += OnChanged;
        }

        private void OnChanged(string id, int value)
        {
            Inventory = _data.Inventory.GetAll();
            SelectedIndex.Value = Mathf.Clamp(SelectedIndex.Value, 0, Inventory.Length - 1);
        }
    }
}
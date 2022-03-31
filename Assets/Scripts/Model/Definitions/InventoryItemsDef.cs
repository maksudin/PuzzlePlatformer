using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/InventoryItems", fileName = "InventoryItems")]
    public class InventoryItemsDef : ScriptableObject
    {
        [SerializeField] private ItemDef[] _items;

#if UNITY_EDITOR
        public ItemDef[] ItemsForEditor => _items;
#endif

        public ItemDef Get(string id)
        {
            foreach (var itemDef in _items)
            {
                if (itemDef.Id == id)
                    return itemDef;
            }
            return default;
        }
    }

    [Serializable]
    public struct ItemDef
    {
        [SerializeField] private string _id;
        [SerializeField] private bool _shouldStack;
        [SerializeField] private int _maxAmount;
        [SerializeField] private Sprite _icon;

        public string Id => _id;
        public bool ShouldStack => _shouldStack;
        public int MaxAmount => _maxAmount;

        public bool IsVoid => string.IsNullOrEmpty(_id);

        public Sprite Icon => _icon;
    }
}
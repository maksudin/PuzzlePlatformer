﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/InventoryItems", fileName = "InventoryItems")]
    public class InventoryItemsDef : ScriptableObject
    {
        [SerializeField] private ItemDef[] _items;

        public ItemDef Get(string id)
        {
            foreach (var itemDef in _items)
            {
                if (itemDef.Id == id)
                    return itemDef;
            }
            return default;
        }

#if UNITY_EDITOR
        public ItemDef[] ItemsForEditor => _items;
#endif
    }

    [Serializable]
    public struct ItemDef
    {
        [SerializeField] private string _id;
        [SerializeField] private int _maxAmount;
        [Header("Icon params")]
        [SerializeField] private Sprite _icon;
        [SerializeField] private float _iconScale;
        [Space]
        [SerializeField] private ItemTag[] _tags;

        public string Id => _id;
        public int MaxAmount => _maxAmount;

        public bool IsVoid => string.IsNullOrEmpty(_id);

        public Sprite Icon => _icon;
        public float IconScale => _iconScale;

        public bool HasTag(ItemTag tag)
        {
            return _tags.Contains(tag);
        }
    }
}
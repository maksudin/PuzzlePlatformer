using System;
using System.Collections.Generic;
using System.Linq;
using PixelCrew.Model.Definitions.Repository;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Items
{
    [CreateAssetMenu(menuName = "Defs/Items", fileName = "Items")]
    public class ItemsDef : DefRepository<ItemDef>
    {
#if UNITY_EDITOR
        public ItemDef[] ItemsForEditor => Collection;
#endif
    }

    [Serializable]
    public struct ItemDef : IHaveId
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
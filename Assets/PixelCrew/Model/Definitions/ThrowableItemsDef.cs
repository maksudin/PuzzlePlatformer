using System;
using PixelCrew.Model.Definitions.Repository.Items;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/ThrowableItems", fileName = "ThrowableItems")]
    public class ThrowableItemsDef : ScriptableObject
    {
        [SerializeField] private ThrowableDef[] _items;

        public ThrowableDef Get(string id)
        {
            foreach (var throwableDef in _items)
            {
                if (throwableDef.ID == id)
                    return throwableDef;
            }
            return default;
        }
    }

    [Serializable]
    public struct ThrowableDef
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private GameObject _projectile;

        public string ID => _id;
        public GameObject Projectile => _projectile;
    }


}
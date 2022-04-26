using System;
using System.Collections.ObjectModel;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repository;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Items
{
    [CreateAssetMenu(menuName = "Defs/ThrowableItems", fileName = "ThrowableItems")]
    public class ThrowableItemsDef : DefRepository<ThrowableDef>
    {
    }

    [Serializable]
    public struct ThrowableDef : IHaveId
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private GameObject _projectile;

        public string Id => _id;
        public GameObject Projectile => _projectile;
    }


}
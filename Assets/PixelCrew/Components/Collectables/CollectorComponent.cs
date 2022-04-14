using System.Collections;
using UnityEngine;
using PixelCrew.Model.Data;
using System.Collections.Generic;
using PixelCrew.Model;

namespace PixelCrew.Components.Collectables
{
    public class CollectorComponent : MonoBehaviour, ICanAddInInventory
    {
        [SerializeField] private List<InventoryItemData> _items = new List<InventoryItemData>();

        public void AddInInventory(string id, int value)
        {

            //сокращение от
            //new InventoryItemData(id).Value = value;  равнло new InventoryItemData (id) {Value = value}
            _items.Add(new InventoryItemData (id) {Value = value});
        }

        public void DropInInventory()
        {
            var session = FindObjectOfType<GameSession>();
            foreach (var item in _items)
            {
                session.Data.Inventory.Add(item.Id, item.Value);
            }
            _items.Clear();
        }
    }
}
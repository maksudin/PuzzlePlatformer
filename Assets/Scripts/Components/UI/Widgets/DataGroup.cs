using System.Collections;
using UnityEngine;

namespace PixelCrew.Components.UI.Widgets
{
    public class DataGroup<TDataType>
    {
        
        //public void SetData(ListM<TDataType> data)
        //{
        //    Debug.Log("Rebuild");
        //    var inventory = _session.QuickInventory.Inventory;

        //    // Create required items.
        //    for (var i = _createdItem.Count; i < inventory.Length; i++)
        //    {
        //        var item = Instantiate(_prefub, _container);
        //        _createdItem.Add(item);
        //    }

        //    // Update data and activate.
        //    for (var i = 0; i < inventory.Length; i++)
        //    {
        //        _createdItem[i].SetData(inventory[i], i);
        //        _createdItem[i].gameObject.SetActive(true);
        //    }

        //    // Hide unused items.
        //    for (var i = inventory.Length; i < _createdItem.Count; i++)
        //    {
        //        _createdItem[i].gameObject.SetActive(false);
        //    }
        }
    }
}
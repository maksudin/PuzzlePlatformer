using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Components.UI.HUD.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private InventoryItemWidget _prefub;

        private GameSession _session;
        private InventoryItemData[] _inventory;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private List<InventoryItemWidget> _createdItem = new List<InventoryItemWidget>();

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _inventory = _session.Data.Inventory.GetAll();

            
            Rebuild();
        }

        private void Rebuild()
        {
            _inventory = _session.Data.Inventory.GetAll();

            // Create required items.
            for (var i = _createdItem.Count; i < _inventory.Length; i++ )
            {
                var item = Instantiate(_prefub, _container);
                _createdItem.Add(item);
            }

            // Update data and activate.
            for (var i = 0; i < _inventory.Length; i++)
            {
                _createdItem[i].SetData(_inventory[i], i);
                _createdItem[i].gameObject.SetActive(true);
            }

            // Hide unused items.
            for (var i = _inventory.Length; i < _createdItem.Count; i++)
            {
                _createdItem[i].gameObject.SetActive(false);
            }
        }
    }
}
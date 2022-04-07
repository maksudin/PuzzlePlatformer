﻿using System.Collections.Generic;
using PixelCrew.Model;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Components.UI.HUD.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private InventoryItemWidget _prefub;

        private GameSession _session;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private List<InventoryItemWidget> _createdItem = new List<InventoryItemWidget>();

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _trash.Retain(_session.QuickInventory.Subscribe(Rebuild));
            Rebuild();
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }

        private void Rebuild()
        {
            Debug.Log("Rebuild");
            var inventory = _session.QuickInventory.Inventory;

            // Create required items.
            for (var i = _createdItem.Count; i < inventory.Length; i++ )
            {
                var item = Instantiate(_prefub, _container);
                _createdItem.Add(item);
            }

            // Update data and activate.
            for (var i = 0; i < inventory.Length; i++)
            {
                _createdItem[i].SetData(inventory[i], i);
                _createdItem[i].gameObject.SetActive(true);
            }

            // Hide unused items.
            for (var i = inventory.Length; i < _createdItem.Count; i++)
            {
                _createdItem[i].gameObject.SetActive(false);
            }
        }
    }
}
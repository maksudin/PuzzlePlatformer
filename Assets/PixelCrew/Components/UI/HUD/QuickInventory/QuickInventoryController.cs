using System.Collections.Generic;
using PixelCrew.Components.UI.Widgets;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Components.UI.HUD.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private InventoryItemWidget _prefab;
        private GameSession _session;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private DataGroup<InventoryItemData, InventoryItemWidget> _dataGroup;

        private void Start()
        {
            _dataGroup = new DataGroup<InventoryItemData, InventoryItemWidget>(_prefab, _container);
            _session = FindObjectOfType<GameSession>();
            _trash.Retain(_session.QuickInventory.Subscribe(Rebuild));
            Rebuild();
        }

        private void OnDestroy() => _trash.Dispose();

        private void Rebuild()
        {
            var inventory = _session.QuickInventory.Inventory;
            _dataGroup.SetData(inventory);
        }
    }
}
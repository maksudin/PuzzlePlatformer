using System.Collections;
using PixelCrew.Components.UI.HUD.QuickInventory;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components.UI.Widgets
{
    public class DataGroup<TDataType, TItemType> where TItemType : MonoBehaviour, IItemRenderer<TDataType>
    {
        private readonly List<TItemType> _createdItem = new List<TItemType>();

        private readonly TItemType _prefab;
        private readonly Transform _container;

        public DataGroup(TItemType prefab, Transform container) {
            _prefab = prefab;
            _container = container;
        }

        public void SetData(IList<TDataType> data)
        {

            // Create required items.
            for (var i = _createdItem.Count; i < data.Count; i++)
            {
                var item = Object.Instantiate(_prefab, _container);
                _createdItem.Add(item);
            }

            // Update data and activate.
            for (var i = 0; i < data.Count; i++)
            {
                _createdItem[i].SetData(data[i], i);
                _createdItem[i].gameObject.SetActive(true);
            }

            // Hide unused items.
            for (var i = data.Count; i < _createdItem.Count; i++)
            {
                _createdItem[i].gameObject.SetActive(false);
            }
        }
    }

    public interface IItemRenderer<TDataType>
    {
        void SetData(TDataType data, int index);
    }
}
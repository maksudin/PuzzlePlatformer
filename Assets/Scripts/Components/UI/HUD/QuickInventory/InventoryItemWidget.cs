using System;
using System.Collections;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;

namespace PixelCrew.Components.UI.HUD.QuickInventory
{
    public class InventoryItemWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _selection;
        [SerializeField] private Text _text;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private int _index = 0;


        private void Start()
        {
            var session = FindObjectOfType<GameSession>();
            session.QuickInventory.SelectedIndex.SubscribeAndInvoke(OnIndexChanged);
        }

        private void OnIndexChanged(int newValue, int _)
        {
            _selection.SetActive(_index == newValue);
        }

        public void SetData(InventoryItemData item, int index)
        {
            _index = index;
            var def = DefsFacade.I.Items.Get(item.Id);

            _icon.sprite = def.Icon;
            if (def.IconScale != 0)
            {
                _icon.transform.localScale = Vector3.one;
                _icon.transform.localScale = new Vector3(_icon.transform.localScale.x * def.IconScale,
                                                         _icon.transform.localScale.y * def.IconScale,
                                                         1);
            }

            if (_text != null)
                _text.text = def.HasTag(ItemTag.Stackable) ? item.Value.ToString() : string.Empty;
        }
    }
}
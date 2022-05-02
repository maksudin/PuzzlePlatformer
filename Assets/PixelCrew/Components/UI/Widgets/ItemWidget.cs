using System;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repository;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.Components.UI.Widgets
{
    public class ItemWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _value;

        public void SetData(ItemWithCount price)
        {
            var def = DefsFacade.I.Items.Get(price.ItemId);
            _icon.sprite = def.Icon;
            _value.text = price.Count.ToString();
        }
    }
}
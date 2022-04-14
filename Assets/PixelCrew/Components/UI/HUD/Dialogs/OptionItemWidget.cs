﻿using System;
using System.Collections;
using PixelCrew.Components.UI.Widgets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PixelCrew.Components.UI.HUD.Dialogs
{
    public class OptionItemWidget : MonoBehaviour, IItemRenderer<OptionData>
    {
        [SerializeField] private Text _label;
        [SerializeField] private SelectOption _onSelect;

        private OptionData _data;

        public void SetData(OptionData data, int index)
        {
            _data = data;
            _label.text = data.Text;
        }

        public void OnSelect()
        {
            _onSelect?.Invoke(_data);
        }

        [Serializable]
        public class SelectOption : UnityEvent<OptionData>
        {

        }
    }
}
using System;
using PixelCrew.Components.UI.Widgets;
using PixelCrew.Model.Definitions.Localization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PixelCrew.Components.UI.Windows.Localization
{
    public class LocaleItemWidget : MonoBehaviour, IItemRenderer<LocaleInfo>
    {
        [SerializeField] private Text _text;
        [SerializeField] private GameObject _selector;
        [SerializeField] private SelectLocale _onSelected;

        private LocaleInfo _data;

        private void Start() =>
            LocalizationManager.I.OnLocaleChanged += UpdateSelection;

        public void SetData(LocaleInfo localInfo, int index)
        {
            _data = localInfo;
            UpdateSelection();
            _text.text = localInfo.LocaleId.ToUpper();
        }

        private void UpdateSelection()
        {
            var isSelected = LocalizationManager.I.LocaleKey == _data.LocaleId;
            _selector.SetActive(isSelected);
        }

        public void OnSelected() =>
            _onSelected?.Invoke(_data.LocaleId);

        private void OnDestroy() =>
            LocalizationManager.I.OnLocaleChanged -= UpdateSelection;
    }

    [Serializable]
    public class SelectLocale : UnityEvent<string> {}

    public class LocaleInfo
    {
        public string LocaleId;
    }
}
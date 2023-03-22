using System;
using Assets.PixelCrew.Model.Data.Properties;
using Assets.PixelCrew.Model.Definitions.Controls;
using PixelCrew.Model;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace Assets.PixelCrew.Model.Data.Models
{
    public class ControlsModel : IDisposable
    {
        public event Action OnChanged;
        public ObservableProperty<string> InterfaceSelectedControl = new ObservableProperty<string>();
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        public ControlsModel(PlayerData data)
        {
            //_data = data;
            _trash.Retain(InterfaceSelectedControl.Subscribe((x, y) => OnChanged?.Invoke()));
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public void RemapButton(string id, KeyCode key)
        {
            ControlsRepository.I.ReplaceKey(id, key);
            OnChanged?.Invoke();
        }

        public void RemapToDefault() { }
        public void Dispose() => _trash.Dispose();
    }
}
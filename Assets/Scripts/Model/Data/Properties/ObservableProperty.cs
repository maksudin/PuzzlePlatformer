using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    [Serializable]
    public abstract class ObservableProperty<TPropertyType>
    {
        [SerializeField] protected TPropertyType _propertyValue;
        private TPropertyType _stored;

        private TPropertyType _defaultValue;

        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);
        public event OnPropertyChanged OnChanged;

        public ObservableProperty(TPropertyType defvalue)
        {
            _defaultValue = defvalue;
        }

        public TPropertyType Value
        {
            get => _stored;
            set
            {
                var isEquals = _stored.Equals(value);
                if (isEquals) return;

                var oldValue = _propertyValue;
                Write(value);
                _stored =_propertyValue = value;

                OnChanged?.Invoke(value, oldValue);
            }
        }

        public void Validate()
        {
            if (!_stored.Equals(_propertyValue))
                Value = _propertyValue;
        }

        protected void Init()
        {
            _stored = _propertyValue = Read(_defaultValue);
        }

        protected abstract void Write(TPropertyType value);
        protected abstract TPropertyType Read(TPropertyType defaultValue);
    }
}
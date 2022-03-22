using System.Collections;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    public abstract class PersistentProperty<TPropertyType>
    {
        [SerializeField] private TPropertyType _propertyValue;
        private TPropertyType _defaultValue;

        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);
        public event OnPropertyChanged OnChanged;

        public PersistentProperty(TPropertyType defvalue)
        {
            _defaultValue = defvalue;
        }

        public TPropertyType Value
        {
            get => _propertyValue;
            set
            {
                //var isEquals = _value.Equals(value);
                if (_propertyValue.Equals(value)) return;

                var oldValue = _propertyValue;
                Write(value);
                _propertyValue = value;

                OnChanged?.Invoke(value, oldValue);
            }
        }

        protected void Init()
        {
            _propertyValue = Read(_defaultValue);
        }

        protected abstract void Write(TPropertyType value);
        protected abstract TPropertyType Read(TPropertyType defaultValue);
    }
}
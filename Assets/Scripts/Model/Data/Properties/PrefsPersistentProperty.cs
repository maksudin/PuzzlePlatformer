using System.Collections;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    public abstract class PrefsPersistentProperty<TPropertyType> : ObservableProperty<TPropertyType>
    {
        protected string Key;

        protected PrefsPersistentProperty(TPropertyType defaultValue, string key) : base(defaultValue)
        {
            Key = key;
        }

    }
}
using System.Collections;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    public class IntProperty : ObservableProperty<int>
    {
        public IntProperty(int defvalue) : base(defvalue)
        {
        }

        protected override int Read(int defaultValue)
        {
            return _propertyValue;
        }

        protected override void Write(int value)
        {
            _propertyValue = value;
        }
    }
}
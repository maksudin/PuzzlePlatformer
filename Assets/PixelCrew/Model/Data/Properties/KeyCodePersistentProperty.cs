using System;
using PixelCrew.Model.Data.Properties;
using UnityEngine;

namespace Assets.PixelCrew.Model.Data.Properties
{
    [Serializable]
    public class KeyCodePersistentProperty : PrefsPersistentProperty<KeyCode>
    {
        public KeyCodePersistentProperty(KeyCode defaultValue, string key) : base(defaultValue, key)
        {
            Init();
        }

        protected override KeyCode Read(KeyCode defaultValue)
        {
            var keyStr = PlayerPrefs.GetString(Key, defaultValue.ToString());
            return (KeyCode)Enum.Parse(typeof(KeyCode), keyStr);
        }

        protected override void Write(KeyCode value)
        {
            PlayerPrefs.SetString(Key, value.ToString());
            PlayerPrefs.Save();
        }
    }
}
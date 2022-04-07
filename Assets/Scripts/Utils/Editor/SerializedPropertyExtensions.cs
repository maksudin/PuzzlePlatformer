using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace PixelCrew.Utils.Editor
{
    public static class SerializedPropertyExtensions
    {
        public static bool GetEnum<TEnumType>(this SerializedProperty property, out TEnumType retValue)
        {
            retValue = default;
            var names = property.enumNames;
            if (names == null || names.Length == 0) 
                return false;

            var enumName = names[property.enumValueIndex];
            retValue = (TEnumType) Enum.Parse(typeof(TEnumType), enumName);
            return true;
        }

    }
}
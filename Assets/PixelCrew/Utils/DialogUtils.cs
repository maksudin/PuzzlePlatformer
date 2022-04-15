using System.Collections;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions.Localization;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Utils
{
    public class DialogUtils
    {
        public static DialogData GetLocalizedDialogData(string[] keys)
        {
            var sentences = new string[keys.Length];
            for (var i = 0; i < keys.Length; i++)
            {
                sentences[i] = LocalizationManager.I.Localize(keys[i]);
            }

            return new DialogData() { Sentences = sentences };
        }
    }
}
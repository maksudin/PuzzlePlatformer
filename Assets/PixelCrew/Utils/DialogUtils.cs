using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions.Localization;

namespace PixelCrew.Utils
{
    public class DialogUtils
    {
        public static DialogData GetLocalizedDialogData(string[] keys)
        {
            Sentence[] sentences = new Sentence[keys.Length];
            for (var i = 0; i < keys.Length; i++)
                sentences[i].ValueId = LocalizationManager.I.Localize(keys[i]);

            return new DialogData() { Sentences = sentences };
        }
    }
}
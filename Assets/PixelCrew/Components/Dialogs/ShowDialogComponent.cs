using System;
using PixelCrew.Components.UI.HUD.Dialogs;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.Dialogs
{
    public class ShowDialogComponent : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [SerializeField] private DialogData _bound;
        [SerializeField] private DialogDef _external;
        [SerializeField] private string[] _keys;

        private DialogData _locales;
        private DialogBoxController _dialogBox;


        public void Show()
        {
            if (_dialogBox == null) 
                _dialogBox = FindObjectOfType<DialogBoxController>();

            _dialogBox.ShowDialog(Data);
        }

        public void Show(DialogDef def)
        {
            _external = def;
            Show();
        }

        public DialogData Data
        {
            get
            {
                switch(_mode)
                {
                    case Mode.Bound:
                        return _bound;
                    case Mode.External:
                        return _external.Data;
                    case Mode.ExternalLocales:
                        return DialogUtils.GetLocalizedDialogData(_keys);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public enum Mode
        {
            Bound,
            External,
            ExternalLocales
        }

    }
}
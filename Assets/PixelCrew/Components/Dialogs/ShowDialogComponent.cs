﻿using System;
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

        private DialogBoxController _dialogBox;

        public void Show()
        {
            _dialogBox = FindDialogController();
            _dialogBox.ShowDialog(Data);
        }

        public void Show(DialogDef def)
        {
            _external = def;
            Show();
        }

        private DialogBoxController FindDialogController()
        {
            if (_dialogBox != null) return _dialogBox;

            GameObject controllerGo;
            switch (Data.Type)
            {
                case DialogType.Simple:
                    controllerGo = GameObject.FindWithTag("SimpleDialog");
                    break;
                case DialogType.Personalized:
                    controllerGo = GameObject.FindWithTag("PersonalizedDialog");
                    break;
                default:
                    throw new ArgumentException("Undefined dialog type");
            }

            return controllerGo.GetComponent<DialogBoxController>();

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
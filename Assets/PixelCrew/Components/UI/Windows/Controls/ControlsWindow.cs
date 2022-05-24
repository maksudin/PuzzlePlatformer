﻿using System;
using Assets.PixelCrew.Model.Definitions.Controls;
using PixelCrew.Components.UI.Widgets;
using PixelCrew.Components.UI.Windows;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.PixelCrew.Components.UI.Windows.Controls
{
    public class ControlsWindow : AnimatedWindow
    {
        [SerializeField] private Transform _controlsContainer;
        [SerializeField] private ControlsWidget _prefab;
        [SerializeField] private Button _okButton;

        private DataGroup<ControlsMappingsDef, ControlsWidget> _dataGroup;

        private GameSession _session;
        private CompositeDisposable _trash = new CompositeDisposable();

        protected override void Start()
        {
            base.Start();

            _dataGroup = new DataGroup<ControlsMappingsDef, ControlsWidget>(_prefab, _controlsContainer);
            _session = FindObjectOfType<GameSession>();

            _trash.Retain(_session.ControlsModel.Subscribe(OnControlsChanged));

            OnControlsChanged();
        }

        private void OnControlsChanged()
        {
            var controls = DefsFacade.I.ControlsRepository.Controls;
            _dataGroup.SetData(controls);
        }

        public void OnDefault()
        {
            _session.ControlsModel.RemapToDefault();
        }

        public void OnRemap()
        {
            var controlsModel = _session.ControlsModel;
            var selected = controlsModel.InterfaceSelectedControl.Value;
            controlsModel.RemapButton(selected);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}
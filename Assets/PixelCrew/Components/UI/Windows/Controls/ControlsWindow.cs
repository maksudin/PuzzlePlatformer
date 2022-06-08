using PixelCrew.Components.UI.Widgets;
using PixelCrew.Components.UI.Windows;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

namespace Assets.PixelCrew.Components.UI.Windows.Controls
{
    public class ControlsWindow : AnimatedWindow
    {
        [SerializeField] private Transform _controlsContainer;
        [SerializeField] private ControlsWidget _prefab;
        [SerializeField] private Button _okButton;
        [SerializeField] private InputActionAsset _heroInputActions;
        [SerializeField] private GameObject _buttonPrompt;

        private InputActionMap _heroActionMap;
        private DataGroup<InputAction, ControlsWidget> _dataGroup;
        private GameSession _session;
        private CompositeDisposable _trash = new CompositeDisposable();
        private RebindingOperation _rebindingOperation;

        private InputAction[] _actions;

        protected override void Start()
        {
            base.Start();
            _heroActionMap = _heroInputActions.FindActionMap("Hero", throwIfNotFound: true);
            _dataGroup = new DataGroup<InputAction, ControlsWidget>(_prefab, _controlsContainer);
            _session = FindObjectOfType<GameSession>();
            _trash.Retain(_session.ControlsModel.Subscribe(OnControlsChanged));

            OnControlsChanged();
        }

        private void OnControlsChanged()
        {
            //var controls = DefsFacade.I.ControlsRepository.Controls;
            var actionsCount = _heroActionMap.actions.Count;
            _actions = new InputAction[actionsCount];

            for (var i = 0; i < actionsCount; i++)
                _actions[i] = _heroActionMap.actions[i];

            _dataGroup.SetData(_actions);
        }

        public void OnDefault()
        {
            var defaultControls = DefsFacade.I.DefaultControlsRepository.DefaultControls;
            foreach (var control in defaultControls)
                _session.ControlsModel.RemapButton(control.Id, control.KeyboardKey);

            OnControlsChanged();
        }

        public void OnRemap()
        {
            var actionName = _session.ControlsModel.InterfaceSelectedControl.Value;
            var action = _heroActionMap.FindAction(actionName);
            if (_buttonPrompt == null) return;

            action.Disable();

            _buttonPrompt.SetActive(true);

            var bindingIndex = 0;
            if (action.bindings[0].isComposite)
            {
                var firstPartIndex = bindingIndex + 1;
                if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isPartOfComposite)
                    PerformInteractiveRebind(action, firstPartIndex, allCompositeParts: true);
            }
            else
            {
                PerformInteractiveRebind(action, bindingIndex);
            }
        }

        private void PerformInteractiveRebind(InputAction action, int bindingIndex, bool allCompositeParts = false)
        {
            _rebindingOperation = action.PerformInteractiveRebinding(bindingIndex)
                .Start()
                .OnCancel(
                    x =>
                    {
                        _buttonPrompt.SetActive(false);
                        _rebindingOperation.Dispose();
                        action.Enable();
                    })
                .OnComplete(
                    x =>
                    {
                        OnControlsChanged();
                        _buttonPrompt.SetActive(false);
                        _rebindingOperation.Dispose();
                        action.Enable();

                        if (allCompositeParts)
                        {
                            var nextBindingIndex = bindingIndex + 1;
                            if (nextBindingIndex < action.bindings.Count && action.bindings[nextBindingIndex].isPartOfComposite)
                                PerformInteractiveRebind(action, nextBindingIndex, true);
                        }
                    });
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}
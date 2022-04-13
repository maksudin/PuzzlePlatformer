using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.Components.UI.HUD.Dialogs
{
    public class OptionDialogController : MonoBehaviour
    {
        [SerializeField] private Text _contextText;
        [SerializeField] private Transform _container;
        [SerializeField] private OptionItemWidget _prefab;

        public void Show(OptionDialogData data)
        {
            _contextText.text = data.DialogText;
        }
    }

    public class OptionDialogData
    {
        public string DialogText;
        public OptionData[] Options;
    }

    [Serializable]
    public class OptionData
    {
        public string Text;
        public UnityEventQueueSystem OnSelect;

    }

}
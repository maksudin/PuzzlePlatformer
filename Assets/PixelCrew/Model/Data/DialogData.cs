using System;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class DialogData
    {
        [SerializeField] private Sentence[] _sentences;
        [SerializeField] private DialogType _type;
        public Sentence[] Sentences { get => _sentences; set => _sentences = value; }
    }

    [Serializable]
    public struct Sentence
    {
        [SerializeField] private string _valueId;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Side _side;

        public string ValueId { get => _valueId; set => _valueId = value; }
        public Sprite Icon => _icon;
        public Side Side => _side;

    }

    public enum Side
    {
        Left,
        Right
    }

    public enum DialogType
    {
        Simple,
        Personalized
    }
}
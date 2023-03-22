using PixelCrew.Components.UI.Widgets;
using PixelCrew.Model;
using PixelCrew.Model.Definitions.Repository;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.Components.UI.Windows.Perks
{
    public class PerkWidget : MonoBehaviour, IItemRenderer<PerkDef>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _isSelected, _isLocked, _isUsed;
        private GameSession _session;
        private PerkDef _data;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            UpdateView();
        }

        public void OnSelect() =>
            _session.PerksModel.InterfaceSelection.Value = _data.Id;

        public void SetData(PerkDef data, int index)
        {
            _data = data;
            if (_session != null)
                UpdateView();
        }

        private void UpdateView()
        {
            _icon.sprite = _data.Icon;
            _isUsed.SetActive(_session.PerksModel.IsUsed(_data.Id));
            _isSelected.SetActive(_session.PerksModel.InterfaceSelection.Value == _data.Id);
            _isLocked.SetActive(!_session.PerksModel.IsUnlocked(_data.Id));
        }
    }
}
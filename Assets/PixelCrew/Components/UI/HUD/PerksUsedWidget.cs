using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.PixelCrew.Components.UI.HUD
{
    public class PerksUsedWidget : MonoBehaviour
    {
        [SerializeField] private GameObject _iconObject;
        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _session.PerksModel.Subscribe(UpdateUsedPerk);
        }

        private void UpdateUsedPerk()
        {
            var perkId = _session.PerksModel.Used;
            if (perkId == "") return;

            _iconObject.SetActive(true);
            _iconObject.GetComponent<Image>().sprite = DefsFacade.I.Perks.Get(perkId).Icon;
        }
    }
}
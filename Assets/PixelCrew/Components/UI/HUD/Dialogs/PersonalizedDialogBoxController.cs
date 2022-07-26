using PixelCrew.Components.UI.HUD.Dialogs;
using PixelCrew.Model.Data;
using UnityEngine;

namespace Assets.PixelCrew.Components.UI.HUD.Dialogs
{
    public class PersonalizedDialogBoxController : DialogBoxController
    {
        [SerializeField] private DialogContent _right;

        protected override DialogContent CurrentContent => CurrentSentence.Side == Side.Left ? Content : _right;

        protected override void OnStartDialogAnimation()
        {
            //Pause();
            _right.gameObject.SetActive(CurrentSentence.Side == Side.Right);
            Content.gameObject.SetActive(CurrentSentence.Side == Side.Left);
            base.OnStartDialogAnimation();
        }

        protected override void OnCloseDialogAnimation()
        {
            //UnPause();
            _right.gameObject.SetActive(false);
            Content.gameObject.SetActive(false);
            base.OnCloseDialogAnimation();
        }
    }
}
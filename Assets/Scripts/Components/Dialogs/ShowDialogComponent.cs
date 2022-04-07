using System.Collections;
using PixelCrew.Model.Data;
using UnityEngine;

namespace PixelCrew.Components.Dialogs
{
    public class ShowDialogComponent : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [SerializeField] private DialogData _bound;
        [SerializeField] private DialogData _external;
        
        public enum Mode
        {
            Bound,
            External
        }

    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.Components.UI.Widgets
{
    public class ProgressBarWidget : MonoBehaviour
    {
        [SerializeField] private Image _bar;

        public void SetProgress(float progress)
        {
            _bar.fillAmount = progress;
            Debug.Log(progress);
        }

    }
}
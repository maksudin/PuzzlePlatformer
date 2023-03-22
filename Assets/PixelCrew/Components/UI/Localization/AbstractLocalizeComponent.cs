using PixelCrew.Model.Definitions.Localization;
using UnityEngine;

namespace PixelCrew.Components.UI.Localization
{
    public abstract class AbstractLocalizeComponent : MonoBehaviour
    {
        protected virtual void Awake()
        {
            LocalizationManager.I.OnLocaleChanged += OnLocaleChanged;
            Localize();
        }

        private void OnDestroy() =>
            LocalizationManager.I.OnLocaleChanged -= OnLocaleChanged;

        private void OnLocaleChanged() => Localize();
        protected abstract void Localize();
    }
}
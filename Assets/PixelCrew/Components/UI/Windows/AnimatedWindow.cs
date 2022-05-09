using UnityEngine;

namespace PixelCrew.Components.UI.Windows
{
    public class AnimatedWindow : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Show = Animator.StringToHash("show");
        private static readonly int Hide = Animator.StringToHash("hide");
        protected float _defaultTimeScale;

        protected virtual void Start()
        {
            _defaultTimeScale = Time.timeScale;
            _animator = GetComponent<Animator>();
            _animator.SetTrigger(Show);
        }

        public void Close()
        {
            _animator.SetTrigger(Hide);
        }

        public virtual void OnCloseAnimationComplete()
        {
            Destroy(gameObject);
        }

        public void Pause()
        {
            Time.timeScale = 0;
        }

        public void UnPause()
        {
            Time.timeScale = _defaultTimeScale;
        }

    }
}
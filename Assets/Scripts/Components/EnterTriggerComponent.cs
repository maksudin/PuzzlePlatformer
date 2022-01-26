using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private UnityEvent _action;
        private bool _passed = false;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_passed)
            {
                if (collision.gameObject.CompareTag(_tag))
                {
                    if (_action != null)
                    {
                        _action.Invoke();
                    }
                }
            }
            else
            {
                return;
            }
            
        }
    }

}

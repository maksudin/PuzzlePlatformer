using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class EnterCollisionComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private EnterEvent _action;
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(_tag))
            {
                if (_action != null)
                {
                    _action.Invoke(collision.gameObject);
                }
            }
        }


        [Serializable]
        public class EnterEvent : UnityEvent<GameObject>
        {
        }

    }

    
}


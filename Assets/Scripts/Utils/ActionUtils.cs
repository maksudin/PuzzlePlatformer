using PixelCrew.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Utils
{
    public class ActionUtils : MonoBehaviour
    {
        private delegate void MethodDelegate();
        MethodDelegate methodDelegate;
        private GameObject _hero;
        private CoinHolder _coinHolder;
        private EnterTriggerComponent _collisionComponent;
        //private UnityAction methodDelegate;
        //private UnityEvent _action;
        private Collision2D collision;
        private Action _action = delegate { };

        private void Start()
        {
            //_hero = GameObject.FindGameObjectsWithTag("Player")[0];
            //_coinHolder = _hero.GetComponent<CoinHolder>();
            //_collisionComponent = GetComponent<EnterTriggerComponent>();
            ////_action = new UnityEvent();
            ////methodDelegate = System.Delegate.CreateDelegate(typeof(UnityAction), _coinHolder, "PickedSilverCoin") as UnityAction;
            //methodDelegate = _coinHolder.PickedSilverCoin;
            //if (methodDelegate != null)
            //{
            //    _action += methodDelegate;
            //}


            ////UnityEditor.Events.UnityEventTools.AddPersistentListener(_action, (UnityAction) methodDelegate);
        }

    }
}


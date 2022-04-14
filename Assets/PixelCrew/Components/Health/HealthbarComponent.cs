using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Components.UI.Widgets;
using PixelCrew.Model.Definitions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.Components.Health
{
    [ExecuteInEditMode]
    public class HealthbarComponent : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private RectTransform _canvasRect;
        [SerializeField] private RectTransform _barTransform;
        private ProgressBarWidget _healthBarWidget;
        
        

        private void Awake()
        {
            _healthBarWidget = GetComponent<ProgressBarWidget>();
        }

        //private void SetHealth(float health, float maxHealth)
        //{
        //    maxHealth = DefsFacade.I.Player.MaxHealth;
        //    var value = (float)health / maxHealth;
        //    _healthBarWidget.SetProgress(value);
        //}

        private void Update()
{
            var targetPosition = Camera.main.WorldToViewportPoint(transform.position);
            var screenPosition = new Vector2(
                //((targetPosition.x * _canvasRect.sizeDelta.x) - (_canvasRect.sizeDelta.x * 0.5f)),
                //((targetPosition.y * _canvasRect.sizeDelta.y) - (_canvasRect.sizeDelta.y * 0.5f))
                (targetPosition.x * _canvasRect.sizeDelta.x) - (_canvasRect.sizeDelta.x * 0.5f),
                (targetPosition.y * _canvasRect.sizeDelta.y) - (_canvasRect.sizeDelta.y * 0.5f)

            );

            //screenPosition += _offset;

            _barTransform.anchoredPosition = targetPosition;
            //SetHealth(_healthComponent.CurrentHealth, _healthComponent.MaxHealth);
        }
    }
}
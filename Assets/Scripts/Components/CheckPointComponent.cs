using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class CheckPointComponent : MonoBehaviour
    {
        [SerializeField] private Transform _checkPoint;
        [SerializeField] private ReloadLevelComponent _reloadLevelComp;
        [SerializeField] private Transform _heroTransform;

        public void InitCheckPoint(Transform checkPoint)
        {
            _checkPoint = checkPoint;
        }

        public void RestartFromCheckPoint()
        {
            if (_checkPoint == null)
            {
                _reloadLevelComp.Reload();
            } else
            {
                _heroTransform.position = _checkPoint.position;
            }
        }
    }
}


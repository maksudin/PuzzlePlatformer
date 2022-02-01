using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class CheckPointComponent : MonoBehaviour
    {
        private Transform _checkPoint;
        [SerializeField] private ReloadLevelComponent _reloadLevelComp;
        [SerializeField] private Transform _heroTransform;

        public void RestartFromCheckPoint(Transform checkPoint)
        {
            _reloadLevelComp.Reload();
            if (checkPoint != null)
            {
                _heroTransform.position = checkPoint.position;
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterReflectComponent : MonoBehaviour
{
    [SerializeField] private float _reflectOffset;
    [SerializeField] private Transform _heroTransform;

    void Update()
    {
        //transform.position = new Vector3(_heroTransform.position.x + _reflectOffset,
        //                                 transform.position.y,
        //                                 transform.position.z);
    }
}

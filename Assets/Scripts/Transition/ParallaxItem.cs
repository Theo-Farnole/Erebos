using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxItem : MonoBehaviour
{
    #region Fields
    [SerializeField] private float _speedFactor = 1;

    private Transform _target;
    private Vector3 _originalPosition;
    #endregion

    #region MonoBehaviour Callbacks
    private void Start()
    {
        _originalPosition = transform.position;
        _target = ParallaxManager.Instance.transform;
    }

    void Update()
    {
        transform.position = _originalPosition + _target.position * _speedFactor;
    }
    #endregion
}

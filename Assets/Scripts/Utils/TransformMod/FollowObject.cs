using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private Transform _target = null;
    private Vector3 _offset = Vector3.one;

    
    void Start()
    {
        _offset = transform.position - _target.position;
    }

    void Update()
    {
        transform.position = _target.position + _offset;        
    }
}

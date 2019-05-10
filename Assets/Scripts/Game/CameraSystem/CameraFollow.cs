using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Fields
    public enum Type { Static, Dynamic }
    public Type _cameraType = Type.Static; // set to public in order to hide variables

    [SerializeField] private bool _followOnX = false;
    [SerializeField] private bool _followOnY = false;
    [Space]
    [Tooltip("Is this the first camera of the level?")]
    [SerializeField] private bool _isTheFirstCamera = false;

    private Transform _target = null;
    private Vector3 _offset = Vector3.one;
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;

        gameObject.SetActive(_isTheFirstCamera);
        UpdateOffset();
    }

    void Update()
    {
        // if camera is static, no need of movements, so no need of Update
        if (_cameraType == Type.Static)
            return;

        Vector3 pos = transform.position;

        if (_followOnX)
        {
            pos.x = _target.position.x + _offset.y;
        }

        if (_followOnY)
        {
            pos.y = _target.position.y + _offset.y;
        }

        transform.position = pos;
    }
    #endregion

    public void UpdateOffset()
    {
        _offset = transform.position - _target.position;
    }
}

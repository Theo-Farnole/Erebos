using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static readonly float MAX_OFFSET = 3f;
    public static readonly float SPEED = 3f;

    #region Fields
    public enum Type { Static, Dynamic }
    public Type _cameraType = Type.Static; // set to public in order to hide variables

    [SerializeField] private bool _followOnX = false;
    [SerializeField] private bool _followOnY = false;
    [Space]
    [Tooltip("Is this the first camera of the level?")]
    [SerializeField] private bool _isActive = false;

    private Transform _target = null;
    private Vector3 _offset = Vector3.one;
    #endregion

    #region Properties
    public bool IsActive
    {
        get
        {
            return _isActive;
        }
        set
        {
            _offset = transform.position - _target.position;
            _isActive = value;
        }
    }
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;

        gameObject.SetActive(_isActive);
        _offset = transform.position - _target.position;
    }

    void Update()
    {
        if (!_isActive)
            return;

        switch (_cameraType)
        {
            case Type.Static:
                break;

            case Type.Dynamic:
                FollowPlayer();
                break;
        }
    }
    #endregion

    void FollowPlayer()
    {
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
}

using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Fields
    public enum Type { Static, Dynamic }
    public Type _cameraType = Type.Static; // set to public in order to hide variables

    [SerializeField] private CameraFollowData _data;
    [Space]
    [SerializeField] private bool _followOnX = false;
    [SerializeField] private bool _followOnY = false;
    [Space]
    [Tooltip("Is this the first camera of the level?")]
    [SerializeField] private bool _isActive = false;

    private Transform _target = null;
    private Vector3 _playerOffset = Vector3.zero;
    private Vector3 _cameraOffset = Vector3.zero;
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
            _playerOffset = transform.position - _target.position;
            _isActive = value;
        }
    }
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        gameObject.SetActive(_isActive);

        _target = GameObject.FindGameObjectWithTag("Player").transform;

        _playerOffset = transform.position - _target.position;
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

        ProcessInput();
    }
    #endregion

    void FollowPlayer()
    {
        Vector3 pos = transform.position;

        if (_followOnX)
        {
            pos.x = _target.position.x + _playerOffset.x;
        }

        if (_followOnY)
        {
            pos.y = _target.position.y + _playerOffset.y;
        }

        transform.position = pos;
    }

    void ProcessInput()
    {
        Vector2 input = GamePad.GetAxis(GamePad.Axis.RightStick, GamePad.Index.Any);                
        Vector3 target = (Vector3)input.normalized * _data.MaxOffset;

        _cameraOffset = Vector3.Slerp(_cameraOffset, target, Time.deltaTime * _data.Speed);
        transform.position += _cameraOffset;
    }
}

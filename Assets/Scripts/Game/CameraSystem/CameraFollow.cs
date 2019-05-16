using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    #region Fields
    public enum Type { Static, Dynamic }

    [Space]
    [SerializeField] private CameraFollowData _data;
    [Space]
    [SerializeField] private Type _cameraType = Type.Static;
    [Tooltip("Is this the first camera of the level?")]
    [SerializeField] private bool _firstCameraOfTheLevel = false;

    private Transform _target = null;
    private Rigidbody _targetRb = null;

    private Vector2 _screenBounds;

    private Vector3 _targetPosition = Vector3.zero;
    private Vector3 _cameraOffset = Vector3.zero;
    #endregion

    #region Properties
    public bool IsActive
    {
        get
        {
            return _firstCameraOfTheLevel;
        }
        set
        {
            _firstCameraOfTheLevel = value;
        }
    }
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _targetRb = _target.GetComponent<Rigidbody>();

        _screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Debug.Log("ScreenBounds: " + _screenBounds);

        gameObject.SetActive(_firstCameraOfTheLevel);
    }

    void LateUpdate()
    {
        if (!_firstCameraOfTheLevel)
            return;

        if (_cameraType == Type.Dynamic)
        {
            SetTargetPosition();
        }

        ProcessInput();
        Move();
    }
    #endregion


    void SetTargetPosition()
    {
        if (_targetRb.velocity.x > 0)
        {
            _targetPosition = _target.position - (0.6f * _screenBounds.x) * Vector3.right;
        }

        else if (_targetRb.velocity.x < 0)
        {
            _targetPosition = _target.position + (0.6f * _screenBounds.x) * Vector3.right;
        }

        else
        {
            // idle
            _targetPosition = _target.position;
        }

        _targetPosition.z = transform.position.z;
        _targetPosition.y += _screenBounds.y / 2;
    }

    void ProcessInput()
    {
        Vector2 input = GamePad.GetAxis(GamePad.Axis.RightStick, GamePad.Index.Any);
        Vector3 target = input.normalized * _data.MaxOffset;

        _cameraOffset = (Vector2)Vector3.Slerp(_cameraOffset, target, Time.deltaTime * _data.Speed);
        _cameraOffset.Clamp(_data.MaxOffset * Vector3.one);
    }

    void Move()
    {
        Vector3 newPosition = Vector3.zero;

        newPosition = Vector3.Slerp(transform.position, _targetPosition, Time.deltaTime * _data.Speed);
        newPosition += _cameraOffset;

        newPosition.z = transform.position.z; // lock Z axis

        transform.position = newPosition;
    }
}

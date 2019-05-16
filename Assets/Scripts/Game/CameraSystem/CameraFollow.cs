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

        float distance = Vector3.Distance(_target.position, transform.position);
        _screenBounds.y = 2.0f * distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad); ;
        _screenBounds.x = _screenBounds.y * Camera.main.aspect; ;
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

        newPosition = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _data.Speed);
        newPosition += _cameraOffset;

        newPosition.z = transform.position.z; // lock Z axis

        transform.position = newPosition;
    }

    private void OnDrawGizmos()
    {
        // draw bounds
        Gizmos.color = Color.yellow;

        Vector3 leftPoint = transform.position - _screenBounds.x * Vector3.right / 2;
        Vector3 rightPoint = transform.position + _screenBounds.x * Vector3.right / 2;
        Gizmos.DrawLine(leftPoint, rightPoint);     // up
        Gizmos.DrawLine(leftPoint - _screenBounds.y * Vector3.up, rightPoint - _screenBounds.y * Vector3.up);    // down

        Gizmos.DrawLine(rightPoint, rightPoint - _screenBounds.y * Vector3.up);    // right
        Gizmos.DrawLine(leftPoint, leftPoint - _screenBounds.y * Vector3.up);    // left


        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _screenBounds.y * Vector3.down);    
    }
}

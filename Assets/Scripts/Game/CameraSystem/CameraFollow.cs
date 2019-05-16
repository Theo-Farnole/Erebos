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
    private Vector2 _targetFocusPosition;
    private Rect _focusRect;

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

        // calculate in game width & height
        float distance = Vector3.Distance(_target.position, transform.position);
        _screenBounds.y = 2.0f * distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad); ;
        _screenBounds.x = _screenBounds.y * Camera.main.aspect;

        // draw focus rect
        _focusRect = new Rect(Vector2.zero, new Vector2(_screenBounds.x * _data.WidthPercent, _screenBounds.y * _data.HeightPercent));
        _focusRect.position = -_focusRect.size * 0.5f;
        _targetFocusPosition = _focusRect.position;

        // set target position
        _targetPosition = _target.position;
        _targetPosition.z = transform.position.z;
        _targetPosition.y += _screenBounds.y / 2;

        gameObject.SetActive(_firstCameraOfTheLevel);
    }

    void LateUpdate()
    {
        if (!_firstCameraOfTheLevel)
            return;

        if (_cameraType == Type.Dynamic)
        {
            SetTargetPosition();
            SetFocusRect();
        }

        //ProcessInput();
        Move();
    }
    #endregion


    void SetTargetPosition()
    {
        float leftDelta = transform.position.x + _focusRect.min.x - _target.position.x;
        float rightDelta = transform.position.x + _focusRect.max.x - _target.position.x;


        if (leftDelta > 0f)
        {
            _targetPosition.x = transform.position.x + _focusRect.min.x;
        }
        else if (rightDelta < 0f)
        {
            _targetPosition.x = transform.position.x + _focusRect.max.x;
        }

        float downDelta = (transform.position.y - _screenBounds.y / 2) + _focusRect.min.y - _target.position.y;
        float upDelta = (transform.position.y - _screenBounds.y / 2) + _focusRect.max.y - _target.position.y;

        if (downDelta > 0f)
        {
            _targetPosition.y = transform.position.y + _focusRect.min.y;
        }
        else if (upDelta < 0f)
        {
            _targetPosition.y = transform.position.y + _focusRect.max.y;
        }

        _targetPosition.z = transform.position.z;
    }

    void SetFocusRect()
    {
        if (_targetRb.velocity.x < 0f)
        {
            _targetFocusPosition = (_screenBounds.x * _data.MaxRectPositionPercent) * Vector2.left - new Vector2(0, _focusRect.size.y * 0.5f);
        }

        else if (_targetRb.velocity.x > 0f)
        {
            _targetFocusPosition = (_screenBounds.x * _data.MaxRectPositionPercent) * Vector2.right - new Vector2(_focusRect.size.x, _focusRect.size.y * 0.5f);
        }

        else
        {
            _targetFocusPosition = -_focusRect.size * 0.5f;
        }

        _focusRect.position = Vector2.Lerp(_focusRect.position, _targetFocusPosition, Time.deltaTime * _data.FocusRectSpeed);
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
        // draw camera's bounds
        Gizmos.color = Color.yellow;

        Vector3 leftPoint = transform.position - _screenBounds.x * Vector3.right / 2;
        Vector3 rightPoint = transform.position + _screenBounds.x * Vector3.right / 2;

        GizmosExtension.Draw2DLine(leftPoint, rightPoint);     // up
        GizmosExtension.Draw2DLine(leftPoint - _screenBounds.y * Vector3.up, rightPoint - _screenBounds.y * Vector3.up);    // down
        GizmosExtension.Draw2DLine(rightPoint, rightPoint - _screenBounds.y * Vector3.up);    // right
        GizmosExtension.Draw2DLine(leftPoint, leftPoint - _screenBounds.y * Vector3.up);    // left

        // draw center
        Gizmos.color = Color.red;
        GizmosExtension.Draw2DLine(transform.position, transform.position + _screenBounds.y * Vector3.down);

        // draw focus rect
        Gizmos.color = Color.green;
        GizmosExtension.DrawRect((Vector2)transform.position + (_screenBounds.y / 2) * Vector2.down, _focusRect);
    }
}

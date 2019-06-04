using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    #region Enums
    public enum Type { Static, Dynamic }
    #endregion

    #region Fields
    [SerializeField] private CameraFollowData _data;
    [SerializeField] private bool _drawDebug = false;
    [Space]
    [SerializeField] private Vector3 _worldMinimumPosition;
    [SerializeField] private Vector3 _worldMaximumPosition;

    private Transform _character = null;
    private Rigidbody _targetRb = null;

    private Vector2 _screenBounds;
    private Vector2 _wantedFocusRelativePosition;
    private Rect _relativeFocusRect;

    private Vector3 _wantedCameraPosition = Vector3.zero;
    private Vector3 _cameraInputOffset = Vector3.zero;

    private float _distanceToTarget = Mathf.Infinity;
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        _character = GameObject.FindGameObjectWithTag("Player").transform;
        _targetRb = _character.GetComponent<Rigidbody>();

        // calculate in game width & height        
        _distanceToTarget = transform.position.z - _character.position.z;
        _screenBounds.y = 2.0f * Mathf.Abs(_distanceToTarget) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        _screenBounds.x = _screenBounds.y * Camera.main.aspect;

        Debug.Log("_screenBounds = " + _screenBounds);

        // draw focus rect
        _relativeFocusRect = new Rect(Vector2.zero, new Vector2(_screenBounds.x * _data.WidthPercent, _screenBounds.y * _data.HeightPercent));

        // reset on death
        RespawnHandle d = new RespawnHandle(CenterOnPlayer);
        CharDeath.EventRespawn += d;

        // on start, center camera on players
        CenterOnPlayer(this);
    }

    void LateUpdate()
    {
        SetFocusRect();
        SetWantedPosition();

        ProcessInput();
        Move();
    }
    #endregion


    void SetFocusRect()
    {
        _wantedFocusRelativePosition = (_screenBounds.x * _data.MaxRectPositionPercent) * Vector2.right;
        _wantedFocusRelativePosition *= Mathf.Sign(_targetRb.velocity.x);

        _relativeFocusRect.center = Vector2.Lerp(_relativeFocusRect.center, _wantedFocusRelativePosition, Time.deltaTime * _data.FocusRectSpeed);
    }

    void SetWantedPosition()
    {
        float leftDelta = transform.position.x + _relativeFocusRect.min.x - _character.position.x;
        float rightDelta = transform.position.x + _relativeFocusRect.max.x - _character.position.x;
        float downDelta = transform.position.y + _relativeFocusRect.min.y - _character.position.y;
        float upDelta = transform.position.y + _relativeFocusRect.max.y - _character.position.y;

        _wantedCameraPosition.x = _character.position.x;
        _wantedCameraPosition.y = _character.position.y + Mathf.Sin(-transform.eulerAngles.x * Mathf.Deg2Rad) * _distanceToTarget;

        if (leftDelta > 0f) _wantedCameraPosition.x -= _relativeFocusRect.min.x;
        else if (rightDelta < 0f) _wantedCameraPosition.x -= _relativeFocusRect.max.x;

        if (downDelta > 0f) _wantedCameraPosition.y -= _relativeFocusRect.max.y / 2;
        else if (upDelta < 0f) _wantedCameraPosition.y -= _relativeFocusRect.min.y / 2;

        _wantedCameraPosition.z = transform.position.z;


        if (leftDelta > 0f) Debug.DrawRay(transform.position, Vector3.right);
        else if (rightDelta < 0f) Debug.DrawRay(transform.position, Vector3.left);
    }

    void ProcessInput()
    {
        float maxOffset = _screenBounds.x * _data.InputPercentOffset;

        Vector2 input = GamePad.GetAxis(GamePad.Axis.RightStick, GamePad.Index.Any);
        Vector3 target = input.normalized * maxOffset;

        _cameraInputOffset = (Vector2)Vector3.Lerp(_cameraInputOffset, target, Time.deltaTime * _data.InputSpeed);
        _cameraInputOffset.Clamp(maxOffset * Vector3.one);
    }

    void Move()
    {
        Vector3 newPosition = Vector3.zero;

        newPosition = Vector3.Lerp(transform.position - _cameraInputOffset, _wantedCameraPosition, Time.deltaTime * _data.Speed);
        newPosition += _cameraInputOffset;

        newPosition.z = transform.position.z; // lock Z axis

        transform.position = newPosition;
    }

    void CenterOnPlayer(object sender)
    {
        // change focus rect
        _wantedFocusRelativePosition = -_relativeFocusRect.size * 0.5f;
        _relativeFocusRect.position = _wantedFocusRelativePosition;

        // change target position
        _wantedCameraPosition.x = _character.position.x;
        _wantedCameraPosition.y = _character.position.y + Mathf.Sin(-transform.eulerAngles.x * Mathf.Deg2Rad) * _distanceToTarget;
        _wantedCameraPosition.z = _distanceToTarget;

        transform.position = _wantedCameraPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(_worldMaximumPosition, 1);
        Gizmos.DrawSphere(_worldMinimumPosition, 1);

        if (!_drawDebug)
            return;

        Vector3 midLeftPoint = transform.position + _screenBounds.x * Vector3.left / 2;
        Vector3 midRightPoint = transform.position + _screenBounds.x * Vector3.right / 2;

        Vector3 topLeftPoint = transform.position + _screenBounds.y * Vector3.up / 2 + _screenBounds.x * Vector3.left / 2;
        Vector3 topRightPoint = transform.position + _screenBounds.y * Vector3.up / 2 + _screenBounds.x * Vector3.right / 2;

        Vector3 bottomLeftPoint = transform.position + _screenBounds.y * Vector3.down / 2 + _screenBounds.x * Vector3.left / 2;
        Vector3 bottomRightPoint = transform.position + _screenBounds.y * Vector3.down / 2 + _screenBounds.x * Vector3.right / 2;

        Vector3 topMidPoint = transform.position + _screenBounds.y * Vector3.up / 2;
        Vector3 bottomMidPoint = transform.position + _screenBounds.y * Vector3.down / 2;

        Vector3 deltaAngle = -Mathf.Sin(-transform.eulerAngles.x * Mathf.Deg2Rad) * _distanceToTarget * Vector3.up;
        midLeftPoint += deltaAngle;
        midRightPoint += deltaAngle;
        topLeftPoint += deltaAngle;
        topRightPoint += deltaAngle;
        bottomLeftPoint += deltaAngle;
        bottomRightPoint += deltaAngle;
        topMidPoint += deltaAngle;
        bottomMidPoint += deltaAngle;

        // draw camera's bounds
        Gizmos.color = Color.yellow;
        GizmosExtension.Draw2DLine(topLeftPoint, topRightPoint);        // up
        GizmosExtension.Draw2DLine(bottomLeftPoint, bottomRightPoint);  // down
        GizmosExtension.Draw2DLine(bottomRightPoint, topRightPoint);    // right
        GizmosExtension.Draw2DLine(bottomLeftPoint, topLeftPoint);      // left

        // draw center
        Gizmos.color = Color.red;
        GizmosExtension.Draw2DLine(bottomMidPoint, topMidPoint); // vertical
        GizmosExtension.Draw2DLine(midLeftPoint, midRightPoint); // horizontal

        // draw focus rect
        Gizmos.color = Color.green;
        GizmosExtension.DrawRect(transform.position + deltaAngle, _relativeFocusRect);

        // draw wanted position
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere((Vector2)_wantedCameraPosition + (Vector2)deltaAngle, 0.5f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere((Vector2)transform.position + _wantedFocusRelativePosition + (Vector2)deltaAngle, 0.5f);
    }
}

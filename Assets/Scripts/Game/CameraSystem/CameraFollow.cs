using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : Singleton<CameraFollow>
{
    #region Fields
    [SerializeField] private CameraFollowData _data;
    [SerializeField] private bool _drawDebug = false;
    [Space]
    [SerializeField] private float _worldMinimumX = Mathf.NegativeInfinity;
    [SerializeField] private float _worldMaximumX = Mathf.Infinity;

    private Transform _character = null;
    private Rigidbody _charRigidbody = null;

    private Vector2 _screenBounds;
    private Rect _focusRect;

    private Vector3 _wantedCameraPositionXY = Vector3.zero;
    private Vector3 _cameraInputOffset = Vector3.zero;

    private float _distanceToTarget = Mathf.Infinity;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        _character = GameObject.FindGameObjectWithTag("Player").transform;
        _charRigidbody = _character.GetComponent<Rigidbody>();

        //_focusRect = ;

        // calculate in game width & height        
        _distanceToTarget = transform.position.z - _character.position.z;
        _screenBounds.y = 2.0f * Mathf.Abs(_distanceToTarget) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        _screenBounds.x = _screenBounds.y * Camera.main.aspect;

        Debug.Log("_screenBounds = " + _screenBounds);
    }

    void Start()
    {
        // reset on death
        RespawnHandle d = new RespawnHandle(CenterOnPlayer);
        CharDeath.EventRespawn += d;

        // on start, center camera on players
        CenterOnPlayer();
    }

    void Update()
    {
        SetWantedPosition();

        //ProcessInput();
        Move();
    }
    #endregion

    void SetWantedPosition()
    {
        // setting X axis
        _wantedCameraPositionXY.x = _character.position.x;

        if (_charRigidbody.velocity.x > 0)
        {
            _wantedCameraPositionXY.x += _screenBounds.x * _data.DeltaFromCenterWidthPercent;
        }
        else if (_charRigidbody.velocity.x < 0)
        {
            _wantedCameraPositionXY.x -= _screenBounds.x * _data.DeltaFromCenterWidthPercent;
        }

        // setting Y axis
        float deltaAngle = -Mathf.Sin(-transform.eulerAngles.x * Mathf.Deg2Rad) * _distanceToTarget;

        bool isPanicLineTop = _character.position.y > transform.position.y + (_data.PanicLineMaxY * _screenBounds.y) + deltaAngle;
        bool isPanicLineBot = _character.position.y < transform.position.y - (_data.PanicLineMinY * _screenBounds.y) + deltaAngle;

        if (isPanicLineBot || isPanicLineTop)
        {
            Debug.Log("CenterONPLayer() panic lines! \nb:" + isPanicLineBot + " t:" + isPanicLineTop);
            
            //_wantedCameraPositionXY.y = _character.position.y + deltaAngle;

            //var pos = transform.position;
            //pos.y = _character.position.y + deltaAngle;
            //transform.position = pos;
        }

        // settings Z axis
        _wantedCameraPositionXY.z = transform.position.z;
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

        float distance = _character.position.x - transform.position.x;
        newPosition.x = transform.position.x + distance / 32f;
        newPosition.x = Mathf.Clamp(newPosition.x, _worldMinimumX, _worldMaximumX);

        newPosition.y = Mathf.Lerp(transform.position.y, _wantedCameraPositionXY.y, _data.Speed.y * Time.deltaTime);
        newPosition.z = transform.position.z;

        transform.position = newPosition;
    }

    public void SmoothCenterOnCharacter()
    {
        // change target position
        _wantedCameraPositionXY.x = _character.position.x;
        _wantedCameraPositionXY.y = _character.position.y + Mathf.Sin(-transform.eulerAngles.x * Mathf.Deg2Rad) * _distanceToTarget;
        _wantedCameraPositionXY.z = _distanceToTarget;
    }

    void CenterOnPlayer()
    {
        SmoothCenterOnCharacter();
        transform.position = _wantedCameraPositionXY;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(Vector3.right * _worldMaximumX + Vector3.up * transform.position.y, 1);
        Gizmos.DrawSphere(Vector3.right * _worldMinimumX + Vector3.up * transform.position.y, 1);

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

        // draw panic line
        var topLeftLine = topLeftPoint;
        var topRightLine = topRightPoint;
        topLeftLine.y = transform.position.y + (_data.PanicLineMaxY * _screenBounds.y) + deltaAngle.y;
        topRightLine.y = transform.position.y + (_data.PanicLineMaxY * _screenBounds.y) + deltaAngle.y;

        var botLeftLine = bottomLeftPoint;
        var botRightLine = bottomRightPoint;
        botLeftLine.y = transform.position.y - (_data.PanicLineMinY * _screenBounds.y) + deltaAngle.y;
        botRightLine.y = transform.position.y - (_data.PanicLineMinY * _screenBounds.y) + deltaAngle.y;

        Gizmos.color = Color.red;
        GizmosExtension.Draw2DLine(topLeftLine, topRightLine);
        GizmosExtension.Draw2DLine(botLeftLine, botRightLine);

        // draw center
        Gizmos.color = Color.red;
        GizmosExtension.Draw2DLine(bottomMidPoint, topMidPoint); // vertical
        GizmosExtension.Draw2DLine(midLeftPoint, midRightPoint); // horizontal

        // draw wanted position
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere((Vector2)_wantedCameraPositionXY + (Vector2)deltaAngle, 0.5f);
    }
}

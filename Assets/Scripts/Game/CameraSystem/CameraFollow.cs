using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : Singleton<CameraFollow>
{
    #region Fields
    public static readonly float MIN_ZOOM_THRESHOLD = 0.5f;

    [SerializeField] private CameraFollowData _data = null;
    [SerializeField] private bool _drawDebug = false;
    [Space]
    [SerializeField] private float _worldMinimumX = Mathf.NegativeInfinity;
    [SerializeField] private float _worldMaximumX = Mathf.Infinity;

    private Transform _cameraContainer = null;

    private Vector3 _wantedCameraPosition = Vector3.zero;
    private Vector3 _cameraInputOffset = Vector3.zero;

    // cached variables
    private Transform _character = null;
    private Rigidbody _charRigidbody = null;
    private CharController _charController = null;

    private Vector2 _screenBounds;
    private float _distanceToTarget = Mathf.Infinity;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        _character = GameObject.FindGameObjectWithTag("Player").transform;
        _charRigidbody = _character.GetComponent<Rigidbody>();
        _charController = _charController.GetComponent<CharController>();

        _wantedCameraPosition = transform.position;

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

        // create camera container
        _cameraContainer = new GameObject
        {
            name = "Camera container"
        }.transform;
        transform.SetParent(_cameraContainer);
    }

    void Update()
    {
        SetWantedPositionX();
        SetWantedPositionY();
        SetWantedPositionZ();

        ProcessInput();
        Move();
    }
    #endregion

    void SetWantedPositionX()
    {
        _wantedCameraPosition.x = _character.position.x;

        if (_charRigidbody.velocity.x > 0)
        {
            _wantedCameraPosition.x += _screenBounds.x * _data.DeltaFromCenterWidthPercent;
        }
        else if (_charRigidbody.velocity.x < 0)
        {
            _wantedCameraPosition.x -= _screenBounds.x * _data.DeltaFromCenterWidthPercent;
        }
    }

    void SetWantedPositionY()
    {
        float deltaAngle = -Mathf.Sin(-transform.eulerAngles.x * Mathf.Deg2Rad) * _distanceToTarget;

        bool isPanicLineTop = _character.position.y > transform.localPosition.y + (_data.PanicLineMaxY * _screenBounds.y) + deltaAngle;
        bool isPanicLineBot = _character.position.y < transform.localPosition.y - (_data.PanicLineMinY * _screenBounds.y) + deltaAngle;

        if (isPanicLineBot || isPanicLineTop)
        {
            _wantedCameraPosition.y = _character.position.y + Mathf.Sin(-transform.eulerAngles.x * Mathf.Deg2Rad) * _distanceToTarget;
        }
    }

    void SetWantedPositionZ()
    {
        // has reached max zoom ?
        if (transform.position.z - (_distanceToTarget - _data.ZoomOut) < MIN_ZOOM_THRESHOLD)
        {
            BackToNormalZoom();
        }
    }

    void ProcessInput()
    {
        // define target position
        float maxOffset = _screenBounds.x * _data.InputPercentOffset;

        Vector2 input = GamePad.GetAxis(GamePad.Axis.RightStick, GamePad.Index.Any);
        Vector3 target = input * maxOffset;

        // apply movement to target position
        _cameraInputOffset = Vector3.Lerp(_cameraInputOffset, target, _data.Speed.z * Time.deltaTime);
        _cameraInputOffset.Clamp(target);

        _cameraInputOffset.z = 0;
    }

    void Move()
    {
        Vector3 newPosition = Vector3.zero;

        float xDistance = _character.position.x - transform.localPosition.x;
        newPosition.x = transform.localPosition.x + xDistance / 32f;
        newPosition.y = Mathf.Lerp(transform.localPosition.y, _wantedCameraPosition.y, _data.Speed.y * Time.deltaTime);
        newPosition.z = Mathf.Lerp(transform.localPosition.z, _wantedCameraPosition.z, _data.Speed.z * Time.deltaTime);

        newPosition.x = Mathf.Clamp(newPosition.x, _worldMinimumX, _worldMaximumX);
        newPosition.z = Mathf.Clamp(newPosition.z, _distanceToTarget - _data.ZoomOut, _distanceToTarget + _data.ZoomIn);

        transform.localPosition = newPosition;

        // process input
        _cameraContainer.position = _cameraInputOffset;

        // override if center need
        if (_charController.CameraShouldCenter)
        {
            SmoothCenterOnCharacter();
        }
    }

    #region Center
    public void SmoothCenterOnCharacter()
    {
        // change target position
        _wantedCameraPosition.x = _character.position.x;
        _wantedCameraPosition.y = _character.position.y + Mathf.Sin(-transform.eulerAngles.x * Mathf.Deg2Rad) * _distanceToTarget;
    }

    void CenterOnPlayer()
    {
        SmoothCenterOnCharacter();

        //_wantedCameraPosition.z = _distanceToTarget;
        transform.position = _wantedCameraPosition;
    }
    #endregion

    #region Zooms & dezoom
    public void BackToNormalZoom()
    {
        _wantedCameraPosition.z = _distanceToTarget;
    }

    public void ZoomIn()
    {
        _wantedCameraPosition.z = _distanceToTarget + _data.ZoomIn;
    }

    public void ZoomOut()
    {
        _wantedCameraPosition.z = _distanceToTarget - _data.ZoomOut;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(Vector3.right * _worldMaximumX + Vector3.up * transform.position.y, 1);
        Gizmos.DrawSphere(Vector3.right * _worldMinimumX + Vector3.up * transform.position.y, 1);

        if (!_drawDebug)
            return;

        Vector3 midLeftPoint = transform.localPosition + _screenBounds.x * Vector3.left / 2;
        Vector3 midRightPoint = transform.localPosition + _screenBounds.x * Vector3.right / 2;

        Vector3 topLeftPoint = transform.localPosition + _screenBounds.y * Vector3.up / 2 + _screenBounds.x * Vector3.left / 2;
        Vector3 topRightPoint = transform.localPosition + _screenBounds.y * Vector3.up / 2 + _screenBounds.x * Vector3.right / 2;

        Vector3 bottomLeftPoint = transform.localPosition + _screenBounds.y * Vector3.down / 2 + _screenBounds.x * Vector3.left / 2;
        Vector3 bottomRightPoint = transform.localPosition + _screenBounds.y * Vector3.down / 2 + _screenBounds.x * Vector3.right / 2;

        Vector3 topMidPoint = transform.localPosition + _screenBounds.y * Vector3.up / 2;
        Vector3 bottomMidPoint = transform.localPosition + _screenBounds.y * Vector3.down / 2;

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
        topLeftLine.y = transform.localPosition.y + (_data.PanicLineMaxY * _screenBounds.y) + deltaAngle.y;
        topRightLine.y = transform.localPosition.y + (_data.PanicLineMaxY * _screenBounds.y) + deltaAngle.y;

        var botLeftLine = bottomLeftPoint;
        var botRightLine = bottomRightPoint;
        botLeftLine.y = transform.localPosition.y - (_data.PanicLineMinY * _screenBounds.y) + deltaAngle.y;
        botRightLine.y = transform.localPosition.y - (_data.PanicLineMinY * _screenBounds.y) + deltaAngle.y;

        Gizmos.color = Color.red;
        GizmosExtension.Draw2DLine(topLeftLine, topRightLine);
        GizmosExtension.Draw2DLine(botLeftLine, botRightLine);

        // draw center
        Gizmos.color = Color.red;
        GizmosExtension.Draw2DLine(bottomMidPoint, topMidPoint); // vertical
        GizmosExtension.Draw2DLine(midLeftPoint, midRightPoint); // horizontal

        // draw wanted position
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere((Vector2)(_wantedCameraPosition + deltaAngle), 0.5f);

        // draw process input
        Gizmos.DrawCube((Vector2)(transform.position + _cameraInputOffset + deltaAngle), Vector3.one * 3);
    }
}

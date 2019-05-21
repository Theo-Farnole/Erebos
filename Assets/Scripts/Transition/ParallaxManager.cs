using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : Singleton<ParallaxManager>
{
    #region Fields
    [SerializeField] private AnimationCurve _speedOnX;
    [SerializeField] private AnimationCurve _speedOnY;
    [Space]
    [SerializeField] private float _animationSeconds = 1f;
    [SerializeField] private float _inputOffset = 10f;
    [SerializeField] private float _inputSpeed = 10f;

    private Vector3 _screenCenter = Vector3.zero;
    private Vector3 _originalPosition = Vector3.zero;
    private Vector3 _wantedInputDelta = Vector3.zero;
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        _screenCenter = new Vector2(Screen.width, Screen.height) / 2;
        _originalPosition = transform.position;
    }

    void Update()
    {
        float horizontal = _speedOnX.Evaluate(Time.timeSinceLevelLoad / _animationSeconds);
        float vertical = _speedOnY.Evaluate(Time.timeSinceLevelLoad / _animationSeconds);

        Vector3 delta = new Vector3(horizontal, vertical, 0);

        if (delta == Vector3.zero)
        {
            Vector3 inputDelta = GamePad.GetAxis(GamePad.Axis.RightStick, GamePad.Index.One) * _inputOffset;
            _wantedInputDelta = Vector3.Lerp(_wantedInputDelta, inputDelta, _inputSpeed * Time.deltaTime);
        }

        _originalPosition += delta;
        transform.position = _originalPosition + _wantedInputDelta;
    }
    #endregion
}

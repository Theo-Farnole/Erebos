using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class CharController : MonoBehaviour
{
    const int MAX_JUMPS = 2;

    #region Fields
    [SerializeField] private float _speed = 3f;
    [Header("Jumps settings")]
    [Range(0, 1)]
    [SerializeField] private float _airControl = 1f;
    [SerializeField] private float _firstJumpForce = 400f;
    [SerializeField] private float _secondJumpForce = 200f;
    [Header("Model settings")]
    [SerializeField] private Transform _model = null;

    // movements variables
    private bool _jumpInput = false;
    private float _horizontal = 0;
    private int _jumpsAvailable = 0;

    // cached variables
    private float _distToGround;
    private Rigidbody _rigidbody;
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    void Update()
    {
        ManageInputs();
    }

    void FixedUpdate()
    {
        ProcessJumpInput();
        ProcessRunInput();
    }
    #endregion

    private void ManageInputs()
    {
        _horizontal = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One).x;
        _jumpInput = GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.One);
    }

    #region Process Methods
    private void ProcessRunInput()
    {
        Vector3 delta = Vector3.right * _horizontal * _speed * Time.fixedDeltaTime;

        if (_jumpsAvailable < MAX_JUMPS) // apply air control
        {
            delta *= _airControl;
        }

        _rigidbody.MovePosition(transform.position + delta);

        // turn the character where he runs
        if (_horizontal != 0)
        {
            Vector3 scale = _model.localScale;
            scale.x = Mathf.Sign(_horizontal);
            _model.localScale = scale;
        }
    }

    private void ProcessJumpInput()
    {
        if (IsGrounded())
        {
            if (_jumpsAvailable != MAX_JUMPS)
            {
                Debug.Log("JUMP RESET");
            }

            _jumpsAvailable = MAX_JUMPS;
        }

        // manage jump input
        if (_jumpInput && _jumpsAvailable > 0)
        {
            if (_jumpsAvailable == 2)       _rigidbody.AddForce(Vector3.up * _firstJumpForce, ForceMode.Impulse);
            else if (_jumpsAvailable == 1)  _rigidbody.AddForce(Vector3.up * _secondJumpForce, ForceMode.Impulse);

            _jumpsAvailable--;
            CharFeedbacks.Instance.PlayJumpPS();

        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, _distToGround);
    }
    #endregion
}

using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class CharController : MonoBehaviour
{
    const int MAX_JUMPS = 2;

    #region Fields
    [SerializeField] private CharControllerData _data;
    [Header("Model settings")]
    [SerializeField] private Transform _model = null;

    // inputs variables
    private bool _jumpInput = false;
    private float _horizontal = 0;

    // movements variables
    private bool _isSticked = false;
    private int _jumpsAvailable = 0;

    // cached variables
    private Rigidbody _rigidbody;
    private Collider _collider;
    private float _distToGround;
    private float _distToSide;
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

        _distToGround = GetComponent<Collider>().bounds.extents.y;
        _distToSide = GetComponent<Collider>().bounds.extents.x;
    }

    void Update()
    {
        ManageInputs();
    }

    void FixedUpdate()
    {
        ProcessRunInput();
        ProcessSticking();
        NormalJump();
    }
    #endregion

    private void ManageInputs()
    {
        _horizontal = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One, true).x;

        if (GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.One))
        {
            _jumpInput = true;
        }

        else if (GamePad.GetButtonUp(GamePad.Button.A, GamePad.Index.One))
        {
            _jumpInput = false;
        }
    }

    private void ProcessRunInput()
    {
        Vector3 speed = Vector3.right * _horizontal * _data.Speed;

        if (!_isSticked)
        {
            Vector3 vel = _rigidbody.velocity;
            vel.x = speed.x;
            _rigidbody.velocity = vel;
        }

        // turn the character where he runs
        if (_horizontal != 0)
        {
            Vector3 scale = _model.localScale;
            scale.x = Mathf.Sign(_horizontal);
            _model.localScale = scale;
        }
    }

    private void ProcessSticking()
    {
        // determinate direction of raycast ...
        Vector3 dir = Vector3.one;

        if (_horizontal < 0)
        {
            dir = Vector3.left;
        }
        else
        {
            dir = Vector3.right;
        }

        // ... then raycast ...
        _isSticked = Physics.Raycast(transform.position, dir, _distToSide + 0.1f);

        // .. reset velocity if raycast is true
        if (_isSticked)
        {
            _rigidbody.velocity = Vector3.one;
        }
    }

    #region Jump Methods
    private void ProcessJumpInput()
    {
        // check if we should reset jumps count.
        if (IsGrounded())
        {
            _jumpsAvailable = MAX_JUMPS;
        }

        if (_isSticked)
        {
            StickedJump();
        }
        else
        {
            NormalJump();
        }
    }



    private void NormalJump()
    {
        if (_jumpInput && _jumpsAvailable > 0)
        {
            _jumpInput = false;

            if (_jumpsAvailable == 2)
            {
                _rigidbody.AddForce(Vector3.up * _data.FirstJumpForce, ForceMode.Impulse);
            }
            else if (_jumpsAvailable == 1)
            {
                _rigidbody.AddForce(Vector3.up * _data.SecondJumpForce, ForceMode.Impulse);
            }

            _jumpsAvailable--;
            CharFeedbacks.Instance.PlayJumpPS();
        }
    }

    private void StickedJump()
    {
        Debug.LogWarning("StickedJump not implemented");
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, _distToGround + 0.1f);
    }
    #endregion
}

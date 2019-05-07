using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class CharController : MonoBehaviour
{
    public static readonly int MAX_JUMPS = 2;

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
        ProcessJumpInput();
    }
    #endregion

    private void ManageInputs()
    {
        _horizontal = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One).x;

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
        if (_horizontal != 0f)
        {
            Vector3 scale = _model.localScale;
            scale.x = Mathf.Sign(_horizontal);
            _model.localScale = scale;
        }
    }

    private void ProcessSticking()
    {
        Debug.Log("Sticked: " + _isSticked);

        // determinate direction of raycast ...
        Vector3 dir = Vector3.zero;

        if (_horizontal < 0f)
        {
            dir = Vector3.left;
        }
        else if (_horizontal > 0f)
        {
            dir = Vector3.right;
        }

        // ... then raycast ...
        if (dir != Vector3.zero)
        {
            Debug.Log("Raycast on sticking coz' ov: " + _horizontal + " && dir: " + dir);
            _isSticked = Physics.Raycast(transform.position, dir, _distToSide + 0.1f);

            // .. reset velocity if raycast is true
            if (_isSticked && false) // tempory disabled
            {
                _rigidbody.velocity = Vector3.zero;
            }
        }
        else
        {
            _isSticked = false;
        }

        _rigidbody.useGravity = !_isSticked;
    }

    #region Jump Methods
    private void ProcessJumpInput()
    {
        // check if we should reset jumps count.
        if (IsGrounded())
        {
            _jumpsAvailable = MAX_JUMPS;
        }

        if (_jumpInput)
        {
            _jumpInput = false;

            if (_isSticked)
            {
                StickedJump();
            }
            else
            {
                NormalJump();
            }
        }
    }

    private void NormalJump()
    {
        if (_jumpsAvailable > 0)
        {
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
        float angle = 0f;

        if (_horizontal < 0f)
        {
            angle = 45f;
        }
        else if (_horizontal > 0f)
        {
            angle = 135f;
        }

        Vector3 dir = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
        _rigidbody.AddForce(dir * _data.StickedJumpForce, ForceMode.Impulse);

        Debug.Log("StickedJump on angle " + angle);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, _distToGround + 0.1f);
    }
    #endregion
}

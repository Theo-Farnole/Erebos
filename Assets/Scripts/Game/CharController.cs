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
        Vector3 vel = _rigidbody.velocity;

        // on ground
        if (_jumpsAvailable == MAX_JUMPS) 
        {
            vel.x = speed.x;
        }
        // air control
        else
        {
            vel.x = speed.x; // tmp var
            //vel.x += speed.x * Time.fixedDeltaTime * _data.AirControl;
        }
        _rigidbody.velocity = vel;

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
            _jumpInput = false;

            Debug.Log("JUMP");
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

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, _distToGround + 0.1f);
    }
}

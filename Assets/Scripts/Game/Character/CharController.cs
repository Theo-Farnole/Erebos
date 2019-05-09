using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class CharController : MonoBehaviour
{
    public static readonly int MAX_JUMPS = 2;

    #region Fields
    class PlayerCollision
    {
        const string FORMAT = "^: {0}, v {1}; {2} < > {3}";

        public bool left = false;
        public bool right = false;
        public bool up = false;
        public bool down = false;

        public PlayerCollision() : this(false, false, false, false)
        {
        }

        public PlayerCollision(bool left, bool right, bool up, bool down)
        {
            this.left = left;
            this.right = right;
            this.up = up;
            this.down = down;
        }

        public override string ToString()
        {
            return string.Format(FORMAT, up, down, left, right);
        }
    }

    [SerializeField] private CharControllerData _data;
    [Header("Model settings")]
    [SerializeField] private Transform _model = null;

    // inputs variables
    private bool _jumpInput = false;
    private float _horizontal = 0;

    // movements variables
    private PlayerCollision _collision = new PlayerCollision();
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
        UpdatePlayerCollisionVar();

        ProcessRunInput();
        ManageSticking();
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

    private void UpdatePlayerCollisionVar()
    {
        _collision.up = Physics.Raycast(transform.position, Vector3.up, _distToGround + 0.1f);
        _collision.down = Physics.Raycast(transform.position, Vector3.down, _distToGround + 0.1f);
        _collision.left = Physics.Raycast(transform.position, Vector3.left, _distToSide + 0.1f);
        _collision.right = Physics.Raycast(transform.position, Vector3.right, _distToSide + 0.1f);
    }

    private void ProcessRunInput()
    {
        // If no collision on side, apply velocity
        if ((_horizontal < 0 && !_collision.left) || (_horizontal > 0 && !_collision.right))
        {
            Vector3 vel = _rigidbody.velocity;

            float acceleration = _collision.down ? _data.Speed : _data.AirControlSpeed;

            vel.x += _horizontal * acceleration;
            vel.x = Mathf.Clamp(vel.x, -_data.MaxVelocityOnX, _data.MaxVelocityOnX);
            _rigidbody.velocity = vel;
        }

        if (_collision.down && _horizontal == 0)
        {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y);
        }

        // turn the character where he runs
        if (_horizontal != 0f)
        {
            Vector3 scale = _model.localScale;
            scale.x = Mathf.Sign(_horizontal);
            _model.localScale = scale;
        }
    }

    #region Sticking Methods
    private void ManageSticking()
    {
        if (_isSticked)
        {
            Unstick();
        }
        else
        {
            Stick();
        }

        _rigidbody.useGravity = !_isSticked;
    }

    private void Stick()
    {
        if (!_collision.down && (_horizontal > 0 && _collision.right) || (_horizontal < 0 && _collision.left))
        {
            _isSticked = true;
            _rigidbody.velocity = Vector3.zero;
        }
    }

    private void Unstick()
    {
        if ((_horizontal > 0 && !_collision.right) || (_horizontal < 0 && !_collision.left))
        {
            _isSticked = false;
        }
    }
    #endregion

    #region Jump Methods
    private void ProcessJumpInput()
    {
        // check if we should reset jumps count.
        if (IsGrounded())
        {
            _jumpsAvailable = MAX_JUMPS;
        }

        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.velocity += Vector3.up * Physics.gravity.y * _data.FallMultiplier * Time.deltaTime;
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
                Vector3 vel = _rigidbody.velocity;
                vel.y = Mathf.Sqrt(2 * _data.FirstJumpHeight * Mathf.Abs(Physics2D.gravity.y));
                _rigidbody.velocity = vel;
            }
            else if (_jumpsAvailable == 1)
            {
                Vector3 vel = _rigidbody.velocity;
                vel.y = Mathf.Sqrt(2 * _data.SecondJumpHeight * Mathf.Abs(Physics2D.gravity.y));
                _rigidbody.velocity = vel;
            }

            _jumpsAvailable--;
            CharFeedbacks.Instance.PlayJumpPS();
        }
    }

    private void StickedJump()
    {
        _isSticked = false;
        float angle = 0f;

        if (_collision.left)
        {
            angle = 45f;
        }
        else if (_collision.right)
        {
            angle = -45f;
        }

        Vector3 dir = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
        _rigidbody.AddForce(dir * _data.StickedJumpForce * _rigidbody.mass, ForceMode.Impulse);

        Debug.Log("StickedJump on angle " + angle);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, _distToGround + 0.1f);
    }
    #endregion
}

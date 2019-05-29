using GamepadInput;
using System;
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

    [SerializeField] private CharControllerData _data = null;
    [Header("Model settings")]
    [SerializeField] private Transform _model = null;


    // inputs variables
    private bool _jumpInput = false;
    private float _horizontal = 0;

    // movements variables
    private PlayerCollision _collision = new PlayerCollision();
    private bool _isSticked = false;
    private bool _isDashing = false;
    private int _jumpsAvailable = 0;
    private bool _isStickingEnable = true;

    // cached variables
    private Rigidbody _rigidbody;
    private Collider _collider;
    private float _distToGround;
    private float _distToSide;
    #endregion

    #region Properties
    public Rigidbody Rigidbody { get => _rigidbody; }
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

        _distToGround = GetComponent<Collider>().bounds.extents.y;
        _distToSide = GetComponent<Collider>().bounds.extents.x;

        DeathHandle d = new DeathHandle(ResetMovements);
        CharDeath.EventDeath += d;
    }

    void Update()
    {
        ManageInputs();
    }

    void FixedUpdate()
    {
        UpdateCollisionsVariable();

        ProcessRunInput();
        ManageSticking();
        ProcessJumpInput();

        _rigidbody.useGravity = !(_isSticked || _isDashing);
    }

    void LateUpdate()
    {
        Vector3 angles = transform.eulerAngles;

        if (_rigidbody.velocity.x < 0f) angles.y = 180;
        else if (_rigidbody.velocity.x > 0f) angles.y = 0;

        transform.eulerAngles = angles;
    }
    #endregion

    private void ManageInputs()
    {
        _horizontal = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One).x;

        if (Time.timeScale != 0 && GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.One))
        {
            _jumpInput = true;
        }

        else if (GamePad.GetButtonUp(GamePad.Button.A, GamePad.Index.One))
        {
            _jumpInput = false;
        }
    }

    private void UpdateCollisionsVariable()
    {
        _collision.up = Physics.Raycast(transform.position, Vector3.up, _distToGround + 0.1f);
        _collision.down = Physics.Raycast(transform.position, Vector3.down, _distToGround + 0.1f);
        _collision.left = Physics.Raycast(transform.position, Vector3.left, _distToSide + 0.1f);
        _collision.right = Physics.Raycast(transform.position, Vector3.right, _distToSide + 0.1f);
    }

    #region Run Methods
    private void ProcessRunInput()
    {
        // If no collision on side, apply velocity
        if (!(_isSticked || _isDashing) && ((_horizontal < 0 && !_collision.left) || (_horizontal > 0 && !_collision.right)))
        {
            Vector3 vel = _rigidbody.velocity;

            float acceleration = _collision.down ? _data.Speed : _data.AirControlSpeed;

            vel.x += _horizontal * acceleration * Time.fixedDeltaTime * _rigidbody.mass;
            vel.x = Mathf.Clamp(vel.x, -_data.MaxVelocityOnX, _data.MaxVelocityOnX);
            _rigidbody.velocity = vel;
        }

        // reset inertia
        if (_isSticked || (_collision.down && _horizontal == 0))
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
    #endregion

    #region Sticking Methods
    private void ManageSticking()
    {
        if (!_isStickingEnable && (_horizontal != 0 || _collision.down))
        {
            _isStickingEnable = true;
        }

        if (_isSticked)
        {
            Unstick();
        }
        else
        {
            Stick();
        }

        // override sticked
        if (!_collision.left && !_collision.right)
        {
            _isSticked = false;
        }
    }

    private void Stick()
    {
        if (_isStickingEnable && !_collision.down && (_collision.right || _collision.left))
        {
            _isSticked = true;
            _rigidbody.velocity = Vector3.zero;
            _jumpsAvailable = 1;

            AudioManager.Instance.PlaySoundGeneral(SoundGeneral.WallGrab);
        }
    }

    private void Unstick()
    {
        if (GamePad.GetButton(GamePad.Button.B, GamePad.Index.Any))
        {
            _isSticked = false;
            _isStickingEnable = false;
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
            _rigidbody.velocity += Vector3.up * Physics.gravity.y * _data.FallMultiplier * Time.fixedDeltaTime;
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
                ProcessGroundedJump();
            }
        }
    }

    private void ProcessGroundedJump()
    {
        if (_jumpsAvailable > 0)
        {
            if (_jumpsAvailable == 2)
            {
                Jump();
                AudioManager.Instance.PlaySoundGeneral(SoundGeneral.Jump);
            }
            else if (_jumpsAvailable == 1)
            {
                Dash();
                AudioManager.Instance.PlaySoundGeneral(SoundGeneral.Dash);
            }

            _jumpsAvailable--;
            CharFeedbacks.Instance.PlayJumpPS();
        }
    }

    private void Jump()
    {
        Vector3 vel = _rigidbody.velocity;
        vel.y = Mathf.Sqrt(2 * _data.JumpHeight * Mathf.Abs(Physics2D.gravity.y));
        _rigidbody.velocity = vel;
    }

    private void Dash()
    {
        // add force
        Vector2 input = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any);
        if (input == Vector2.zero) input = Vector2.up;// if no input, dash on up

        Vector3 force = (input.normalized * _data.DashDistance) / _data.DashTime;

        _rigidbody.velocity = force;

        // dash boolean
        _isDashing = true;
        StartCoroutine(CustomDelay.ExecuteAfterTime(_data.DashTime, () =>
        {
            _isDashing = false;
            _rigidbody.velocity = Vector3.zero;
        }
        ));

        // debug
        GizmosPersistence.DrawPersistentLine(transform.position, transform.position + force);
    }

    private void StickedJump()
    {
        _isSticked = false;
        float angle = 0f;

        if (_collision.left)
        {
            angle = _data.StickedJumpAngle;
        }
        else if (_collision.right)
        {
            angle = -_data.StickedJumpAngle;
        }

        _jumpsAvailable = 1;

        Vector3 dir = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
        _rigidbody.AddForce(dir * _data.StickedJumpForce * _rigidbody.mass, ForceMode.Impulse);

        AudioManager.Instance.PlaySoundGeneral(SoundGeneral.WallJump);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, _distToGround + 0.1f);
    }
    #endregion

    public void ResetMovements(object sender = null)
    {
        _rigidbody.velocity = Vector3.zero;

        _isSticked = false;
        _isStickingEnable = true;
        _isDashing = false;

        // put dash
        _jumpsAvailable = 1;
    }
}

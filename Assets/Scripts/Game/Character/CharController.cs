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
    [SerializeField] private Animator _animator = null;

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

    private readonly int _hashWalkBlend = Animator.StringToHash("WalkBlend");
    private readonly int _hashJump = Animator.StringToHash("Jump");
    private readonly int _hashFall = Animator.StringToHash("Fall");
    private readonly int _hashGrounded = Animator.StringToHash("Grounded");
    private readonly int _hashIsInAir = Animator.StringToHash("IsInAir");
    private readonly int _hashWallGrab = Animator.StringToHash("WallGrab");
    private readonly int _hashUnstick = Animator.StringToHash("Unstick");
    private readonly int _hashWallJump = Animator.StringToHash("Walljump");
    private readonly int _hashDeath = Animator.StringToHash("Death");
    private readonly int _hashVelocity = Animator.StringToHash("Velocity");
    private readonly int _hashLeftCollision = Animator.StringToHash("LeftCollision");
    #endregion

    #region Properties
    public Rigidbody Rigidbody { get => _rigidbody; }
    public int JumpsAvailable
    {
        get
        {
            return _jumpsAvailable;
        }

        set
        {
            _jumpsAvailable = value;

            CharFeedbacks.Instance.UpdateFormMaterial(_jumpsAvailable >= 1);
        }
    }
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

        _distToGround = GetComponent<Collider>().bounds.extents.y;
        _distToSide = GetComponent<Collider>().bounds.extents.x;
    }

    void Start()
    {
        DeathHandle d1 = new DeathHandle(() =>
        {
            ResetMovements();
            _rigidbody.useGravity = false;
            _animator.SetTrigger(_hashDeath);
        });
        CharDeath.EventDeath += d1;
    }

    void Update()
    {
        if (CharDeath.isDead)
            return;

        if (_collision.down || _isSticked)
        {
            CameraFollow.Instance.SmoothCenterOnCharacter();
        }

        ManageInputs();
    }

    void FixedUpdate()
    {
        if (CharDeath.isDead)
            return;

        UpdateCollisionsVariable();

        ProcessRunInput();
        ManageSticking();
        ProcessJumpInput();

        _rigidbody.useGravity = !(_isSticked || _isDashing);
    }

    void LateUpdate()
    {
        LookAtDirection();
        SetAnimatorValue();
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

        // if sticked or not runnings
        if (_isSticked || (_collision.down && _horizontal == 0))
        {
            // reset inertia
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y);
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
            JumpsAvailable = 1;

            AudioManager.Instance.PlaySoundGeneral(SoundGeneral.WallGrab);
        }
    }

    private void Unstick()
    {
        if (GamePad.GetButton(GamePad.Button.B, GamePad.Index.Any))
        {
            _animator.SetTrigger(_hashUnstick);

            _isSticked = false;
            _isStickingEnable = false;
        }
    }
    #endregion

    #region Jump Methods
    private void ProcessJumpInput()
    {
        // check if we should reset jumps count.
        if (_collision.down)
        {
            JumpsAvailable = MAX_JUMPS;
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
        if (JumpsAvailable > 0)
        {
            if (JumpsAvailable == 2)
            {
                Jump();
                AudioManager.Instance.PlaySoundGeneral(SoundGeneral.Jump);
            }
            else if (JumpsAvailable == 1)
            {
                Dash();
                AudioManager.Instance.PlaySoundGeneral(SoundGeneral.Dash);
            }

            JumpsAvailable--;
        }
    }

    private void Jump()
    {
        _animator.SetTrigger(_hashJump);

        Vector3 vel = _rigidbody.velocity;
        vel.y = Mathf.Sqrt(2 * _data.JumpHeight * Mathf.Abs(Physics2D.gravity.y));
        _rigidbody.velocity = vel;

        CharFeedbacks.Instance.PlayJumpPS();
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

            CharFeedbacks.Instance.StopDashSequence();
        }
        ));

        // feedback
        float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
        CharFeedbacks.Instance.PlayDashSequence(angle);
    }

    private void StickedJump()
    {
        _animator.SetTrigger(_hashWallJump);
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

        JumpsAvailable = 1;

        Vector3 dir = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
        _rigidbody.AddForce(dir * _data.StickedJumpForce * _rigidbody.mass, ForceMode.Impulse);

        AudioManager.Instance.PlaySoundGeneral(SoundGeneral.WallJump);
    }
    #endregion

    public void ResetMovements()
    {
        _rigidbody.velocity = Vector3.zero;

        _isSticked = false;
        _isStickingEnable = true;
        _isDashing = false;

        // put dash
        JumpsAvailable = 1;
    }

    #region Animation Methods
    void LookAtDirection()
    {
        // if in pause, don't look at direction
        if (Time.timeScale == 0)
            return;

        Vector3 angles = transform.eulerAngles;
        Vector3 localScale = transform.localScale;

        if (_isSticked)
        {
            if (_collision.left) localScale.x = Mathf.Abs(localScale.x) * -1;
            if (_collision.right) localScale.x = Mathf.Abs(localScale.x) * 1;
        }
        else
        {
            localScale.x = Mathf.Abs(localScale.x);

            if (_rigidbody.velocity.x < 0f) angles.y = 180;
            if (_rigidbody.velocity.x > 0f) angles.y = 0;
        }

        transform.eulerAngles = angles;
        transform.localScale = localScale;
    }

    void SetAnimatorValue()
    {
        // if in pause, don't update animation
        if (Time.timeScale == 0)
            return;

        bool isFalling = _rigidbody.velocity.y < 0;
        bool isGrounded = _collision.down;
        float velocity = 0;

        if (_rigidbody.velocity.x > 0) velocity = 1;
        if (_rigidbody.velocity.x < 0) velocity = -1;

        _animator.SetBool(_hashFall, isFalling);
        _animator.SetBool(_hashGrounded, isGrounded);
        _animator.SetFloat(_hashWalkBlend, Mathf.Abs(_horizontal));
        _animator.SetBool(_hashIsInAir, _rigidbody.velocity.x != 0);
        _animator.SetBool(_hashWallGrab, _isSticked);
        _animator.SetFloat(_hashVelocity, velocity);
        _animator.SetBool(_hashLeftCollision, _collision.left);
    }

    public void TriggerJump()
    {
        _animator.SetTrigger(_hashJump);
    }
    #endregion
}

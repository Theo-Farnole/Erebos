using Erebos.Inputs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.CompilerServices;
using UnityEngine;

// POST MORTEM NOTE
// I should have made this class in another way.
//
// By using a state machine (Dash, Stick, Attracted, etc..), 
// I would have a smallest classes and more maintenable code.


[SelectionBase]
public class CharController : MonoBehaviour
{
    public static readonly int MAX_JUMPS = 2;
    public static readonly Vector3 HEAD_POSITION = 0.7f * Vector3.up;
    public static readonly float RADIUS_GROUNDED_SPHERE = 0.4f;
    public static readonly float FALL_TRIGGER_MIN_Y = -0.1f;

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
    private Coroutine _dashCoroutine = null;

    // cached variables
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    private float _distToGround;
    private float _distToSide;
    private Vector3 _sphereOverlapPosition;

    private int _layerMask;

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
    private readonly int _hashSwitchForm = Animator.StringToHash("SwitchForm");
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
    
    public bool CameraShouldCenter
    {
        get
        {
            return (_collision.down || _isSticked);
        }
    }

    public bool IsBlocked
    {
        get
        {
            //Debug.Log("_collision " + _collision);

            if (_collision.left && _collision.right && _collision.up && _collision.down
                && _rigidbody.velocity == Vector3.zero)
            {
                Debug.Log("Player get killed because he's block");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();

        _distToGround = _collider.bounds.extents.y;
        _distToSide = _collider.bounds.extents.x;
        _sphereOverlapPosition = ((_distToGround + 0.1f) - RADIUS_GROUNDED_SPHERE) * Vector3.down;

        _layerMask = ~LayerMask.GetMask("Player");
    }

    void Start()
    {
        DeathHandle d1 = new DeathHandle(() =>
        {
            ResetMovements();

            EndDash(false);

            _rigidbody.useGravity = false;
            _animator.SetTrigger(_hashDeath);
        });
        CharDeath.EventDeath += d1;

        FormHandle d2 = new FormHandle((Form form) => _animator.SetTrigger(_hashSwitchForm));
    }

    void Update()
    {
        if (CharDeath.isDead)
            return;

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

        if (_isDashing && (_collision.left || _collision.right))
        {
            EndDash(false);
        }

        SetAnimatorValue();
    }

    void LateUpdate()
    {
        LookAtDirection();

        DisableGC();
    }
    #endregion

    private void ManageInputs()
    {
        _horizontal = InputProxy.Character.Horizontal;

        if (Time.timeScale != 0 && InputProxy.Character.JumpDown)
        {
            _jumpInput = true;
        }

        else if (InputProxy.Character.JumpUp)
        {
            _jumpInput = false;
        }
    }

    private void UpdateCollisionsVariable()
    {
        _collision.up = Physics.Raycast(transform.position, Vector3.up, _distToGround + 0.1f, _layerMask);
        _collision.left = Physics.Raycast(transform.position + HEAD_POSITION, Vector3.left, _distToSide + 0.1f, _layerMask);
        _collision.right = Physics.Raycast(transform.position + HEAD_POSITION, Vector3.right, _distToSide + 0.1f, _layerMask);

        var colliders = Physics.OverlapSphere(transform.position + _sphereOverlapPosition, RADIUS_GROUNDED_SPHERE, _layerMask);
        _collision.down = colliders.Length > 0;
    }

    #region Run Methods
    private void ProcessRunInput()
    {
        // If no collision on side, apply velocity
        if (!(_isSticked || _isDashing) && ((_horizontal < 0 && !_collision.left) || (_horizontal > 0 && !_collision.right)))
        {
            Vector3 vel = _rigidbody.velocity;

            if (_collision.down)
            {
                vel.x = _horizontal * _data.MaxVelocityOnX;
            }
            else
            {
                vel.x += _horizontal * _data.AirControlSpeed * Time.fixedDeltaTime * _rigidbody.mass;
            }

            vel.x = Mathf.Clamp(vel.x, -_data.MaxVelocityOnX, _data.MaxVelocityOnX);
            _rigidbody.velocity = vel;

            if (_collision.down)
            {
                AudioManager.Instance.PlayFootsteps();
            }
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
            _rigidbody.velocity = Vector3.zero;

            CheckIfUnsticked();
        }
        else
        {
            CheckIfSticked();
        }

        // override sticked
        if (!_collision.left && !_collision.right)
        {
            _isSticked = false;
        }
    }

    private void CheckIfSticked()
    {
        if (_isStickingEnable && !_collision.down && (_collision.right || _collision.left))
        {
            _isSticked = true;
            _rigidbody.velocity = Vector3.zero;
            JumpsAvailable = 1;

            AudioManager.Instance.PlaySoundGeneral(SoundGeneral.WallGrab);
        }
    }

    private void CheckIfUnsticked()
    {
        if (InputProxy.Character.Unstick)
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
        Vector2 input = InputProxy.Character.LeftInput;
        if (input == Vector2.zero) input = Vector2.up;// if no input, dash on up

        Vector3 force = (input.normalized * _data.DashDistance) / _data.DashTime;
        _rigidbody.velocity = force;

        // update collider size
        _collider.height = 1;

        // dash boolean
        _isDashing = true;
        _dashCoroutine = this.ExecuteAfterTime(_data.DashTime, () => EndDash(true));

        // feedback
        float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
        CharFeedbacks.Instance.PlayDashSequence(angle);
    }

    private void EndDash(bool addVelocity)
    {
        _isDashing = false;

        StopCoroutine(_dashCoroutine);
        CharFeedbacks.Instance.StopDashSequence();

        // back to normal size
        _collider.height = 2;

        // add velocity or reset
        Vector3 addVelocityVector = _rigidbody.velocity.normalized * _data.DashInertia;
        _rigidbody.velocity = addVelocity ? addVelocityVector : Vector3.zero;
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

        Vector3 angles = _model.eulerAngles;
        Vector3 localScale = _model.localScale;

        if (_isSticked)
        {
            //angles.y = 180;

            if (_collision.left)
            {
                angles.y = -90;
                localScale.x = Mathf.Abs(localScale.x) * -1;
            }
            if (_collision.right)
            {
                angles.y = 90;
                localScale.x = Mathf.Abs(localScale.x) * 1;
            }
        }
        else
        {
            localScale.x = Mathf.Abs(localScale.x);

            if (_rigidbody.velocity.x < 0f) angles.y = -90;
            if (_rigidbody.velocity.x > 0f) angles.y = 90;
        }

        _model.eulerAngles = angles;
        _model.localScale = localScale;
    }

    void SetAnimatorValue()
    {
        // if in pause, don't update animation
        if (Time.timeScale == 0)
            return;

        bool isGrounded = _collision.down;
        bool isFalling = !isGrounded && _rigidbody.velocity.y < 0;
        float velocity = 0;

        if (_rigidbody.velocity.x > 0) velocity = 1;
        if (_rigidbody.velocity.x < 0) velocity = -1;

        _animator.SetBool(_hashFall, isFalling);
        _animator.SetBool(_hashGrounded, isGrounded);
        _animator.SetFloat(_hashWalkBlend, Mathf.Abs(_horizontal));
        _animator.SetBool(_hashIsInAir, !isGrounded);
        _animator.SetBool(_hashWallGrab, _isSticked);
        _animator.SetFloat(_hashVelocity, velocity);
        _animator.SetBool(_hashLeftCollision, _collision.left);
    }

    public void TriggerJump()
    {
        _animator.SetTrigger(_hashJump);
    }
    #endregion

    /// <summary>
    /// Avoid GC calls when the player is in the air
    /// </summary>
    private void DisableGC()
    {
        bool isInAir = _rigidbody.velocity.x != 0;

        GCLatencyMode wantedMod = isInAir ? GCLatencyMode.LowLatency : GCLatencyMode.Interactive;


        GCLatencyMode oldMode = GCSettings.LatencyMode;

        // Make sure we can always go to the catch block, 
        // so we can set the latency mode back to `oldMode`
        RuntimeHelpers.PrepareConstrainedRegions();

        try
        {
            GCSettings.LatencyMode = wantedMod;

            // Generation 2 garbage collection is now
            // deferred, except in extremely low-memory situations
        }
        finally
        {
            // ALWAYS set the latency mode back
            GCSettings.LatencyMode = oldMode;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position + _sphereOverlapPosition, RADIUS_GROUNDED_SPHERE);
    }
}

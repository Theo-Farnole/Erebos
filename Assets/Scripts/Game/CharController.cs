using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    #region Fields
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _jumpForce = 300f;
    [Space]
    [SerializeField] private Transform _model = null;

    // jumps var
    private bool _isJumping = false;
    private bool _canJump = true;

    // cached var
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
        ManageJump();
        ManageRun();
    }
    #endregion

    private void ManageRun()
    {
        float horizontal = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One).x;

        // make move the character
        Vector3 delta = Vector3.right * horizontal * _speed * Time.deltaTime;
        _rigidbody.MovePosition(transform.position + delta);

        // turn the character where he runs
        if (horizontal != 0)
        {
            Vector3 scale = _model.localScale;
            scale.x = Mathf.Sign(horizontal);
            _model.localScale = scale;
        }
    }

    private void ManageJump()
    {
        if (IsGrounded())
        {
            _canJump = true;
            _isJumping = false;
        }

        bool jumpInput = GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.One);

        if (jumpInput && _canJump)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, _distToGround);
    }
}

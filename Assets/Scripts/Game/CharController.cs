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
    [SerializeField] private float _firstJumpForce = 400f;
    [SerializeField] private float _secondJumpForce = 200f;
    [Space]
    [SerializeField] private Transform _model = null;

    // jumps var
    private int _jumpsAvailable = 0;

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

    void FixedUpdate()
    {
        ManageJump();
        ManageRun();
    }
    #endregion

    private void ManageRun()
    {
        float horizontal = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One).x;

        // make move the character
        Vector3 delta = Vector3.right * horizontal * _speed * Time.fixedDeltaTime;
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
            if (_jumpsAvailable != MAX_JUMPS)
            {
                Debug.Log("JUMP RESET");
            }

            _jumpsAvailable = MAX_JUMPS;
        }

        // manage jump input
        bool jumpInput = GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.One);

        if (jumpInput && _jumpsAvailable > 0)
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
}

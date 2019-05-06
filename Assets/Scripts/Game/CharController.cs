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

    private float _distToGround;
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        // get the distance to ground
        _distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    void Update()
    {
        ManageRun();
        ManageJump();
    }
    #endregion

    private void ManageRun()
    {
        float horizontal = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One).x;

        // make move the character
        transform.position += Vector3.right * horizontal * _speed * Time.deltaTime;

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
        bool jumpInput = GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.One);

        if (jumpInput && IsGrounded())
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * _jumpForce);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, _distToGround);
    }
}

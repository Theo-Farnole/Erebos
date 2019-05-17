using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSingularity : AbstractSingularity
{
    #region Fields
    [SerializeField] private BlackSingularityData _data;
    [SerializeField] private DrawCircle _rangeFeedback;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        GetComponent<SphereCollider>().radius = _data.Radius;
        UpdateRangeFeedback();
    }
    #endregion

    #region Overrided Methods
    protected override void OnStay()
    {
        if (Vector3.Distance(transform.position, _character.position) <= _data.CharacterRotateRadius)
        {
            RotateCharacter();
        }
        else
        {
            AttractPlayer();
        }
    }

    protected override void OnExit()
    {
        //throw new System.NotImplementedException();
    }
    #endregion

    // note: this function shouldn't be in this script, but in the CharControllerSingularity!
    private void RotateCharacter()
    {
        _character.GetComponent<Rigidbody>().velocity = Vector3.zero;
        int anglePerSecond = 360;


        // process input
        float horizontal = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One).x * Time.deltaTime * anglePerSecond;

        Vector3 dir = _character.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        angle += horizontal;

        // apply new position
        Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        _character.position = transform.position + offset * _data.CharacterRotateRadius;

    }

    private void AttractPlayer()
    {
        Vector3 dir = (transform.position - _character.position).normalized;
        float speed = _data.Radius / _data.TimeToReachCenter;

        // apply velocity
        Vector3 vel = speed * dir;
        _character.GetComponent<Rigidbody>().velocity = vel * Time.deltaTime;
        //_character.GetComponent<Rigidbody>().velocity.Clamp(vel);

        // debug
        Debug.DrawRay(transform.position, dir * speed, Color.red);
    }

    public void UpdateRangeFeedback()
    {
        if (_rangeFeedback == null)
        {
            Debug.LogError(transform.name + " has no range feedback dragged!");
            return;
        }

        _rangeFeedback.xradius = _data.Radius;
        _rangeFeedback.yradius = _data.Radius;

        _rangeFeedback.UpdateCircle();
    }
}

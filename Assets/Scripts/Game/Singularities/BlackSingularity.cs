using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
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
            _character.SetParent(transform);
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

        Vector2 input = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One);

        if (input != Vector2.zero)
        {
            float radiansSpeed = 180 * Mathf.Deg2Rad; // in rads

            // calculate current angle
            Vector3 characterDirection = (_character.position - transform.position).normalized;
            float currentAngle = Mathf.Atan2(characterDirection.y, characterDirection.x); // in radians
            if (currentAngle < 0f) currentAngle += 360f;

            // calcule input angle
            float angleInput = Mathf.Atan2(input.y, input.x); // in radians
            if (angleInput < 0f) angleInput += 360f;

            // calculate angle delta
            float angleDelta = angleInput - currentAngle; // in radians

            float finalAngle = currentAngle + Mathf.Clamp(angleDelta, -1, 1) * Time.deltaTime * radiansSpeed;
            Vector3 newPosition = new Vector3(Mathf.Cos(finalAngle), Mathf.Sin(finalAngle));

            _character.position = transform.position + newPosition;

            // debugs
            //Debug.Log("AngleInput: " + angleInput + "\ncurrentAngle " + currentAngle);
            Debug.DrawRay(transform.position, characterDirection);
            Debug.DrawRay(transform.position, input);
        }
    }

    private void AttractPlayer()
    {
        Vector3 dir = (transform.position - _character.position).normalized;
        float speed = _data.Radius / _data.TimeToReachCenter;

        // apply velocity
        Vector3 vel = speed * dir;
        _character.GetComponent<Rigidbody>().velocity = vel * Time.deltaTime;
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

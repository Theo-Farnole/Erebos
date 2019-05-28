using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class BlackSingularity : AbstractSingularity
{
    #region Fields
    [SerializeField] private BlackSingularityData _data;

    private float _currentAngleDelta = Mathf.Infinity;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        GetComponent<SphereCollider>().radius = _data.Radius;
    }

    private void OnEnable()
    {
        transform.eulerAngles = Vector3.zero;
    }

    private void OnDisable()
    {
        _currentAngleDelta = Mathf.Infinity; // reset variable
    }
    #endregion

    #region Overrided Methods
    protected override void OnEnter() { }

    protected override void OnStay()
    {
        _character.GetComponent<CharControllerManager>().Attracted = true;

        if (Vector3.Distance(transform.position, _character.position) <= _data.CharacterRotateRadius)
        {
            CalculateCurrentAngleDelta();

            _character.SetParent(transform);
            _character.GetComponent<CharControllerSingularity>().RotateAroundSingularity(transform, _currentAngleDelta);
        }
        else
        {
            AttractPlayer();
        }
    }
    #endregion

    private void AttractPlayer()
    {
        Vector3 dir = (transform.position - _character.position).normalized;
        float speed = _data.Radius / _data.TimeToReachCenter;

        // apply velocity
        Vector3 vel = speed * dir;
        _character.GetComponent<Rigidbody>().velocity = vel * Time.deltaTime;
    }

    private void CalculateCurrentAngleDelta()
    {
        // calculate only if current angle delta isn't calculated
        if (_currentAngleDelta != Mathf.Infinity)
            return;

        Vector3 dir = _character.position - transform.position;
        _currentAngleDelta = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
}

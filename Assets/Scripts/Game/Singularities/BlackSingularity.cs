using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSingularity : AbstractSingularity
{
    #region Fields
    [SerializeField] private BlackSingularityData _data;
    [SerializeField] private DrawCircle _rangeFeedback;

    private Transform _player;
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
        AttractPlayer();
    }

    protected override void OnExit()
    {
        //throw new System.NotImplementedException();
    }
    #endregion

    private void AttractPlayer()
    {
        Vector3 dir = (transform.position - _player.position).normalized;
        float speed = _data.Radius / _data.TimeToReachCenter;

        // apply velocity
        Vector3 vel = _player.GetComponent<Rigidbody>().velocity;

        vel = dir * Time.deltaTime * Mathf.Sqrt(2 * speed * Mathf.Abs(Physics2D.gravity.y)); ;

        _player.GetComponent<Rigidbody>().velocity = vel;

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

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
        AttractPlayer();
    }

    protected override void OnExit()
    {
        //throw new System.NotImplementedException();
    }
    #endregion

    private void AttractPlayer()
    {
        Debug.Log("AttractPlayer()");

        Vector3 dir = (transform.position - _character.position).normalized;
        float speed = _data.Radius / _data.TimeToReachCenter;

        // apply velocity
        Vector3 vel = speed * dir * Time.deltaTime; 
        _character.GetComponent<Rigidbody>().velocity = vel;

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

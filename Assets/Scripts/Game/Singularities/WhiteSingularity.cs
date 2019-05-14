using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteSingularity : AbstractSingularity
{
    #region Fields
    [SerializeField] private WhiteSingularityData _data;
    #endregion

    #region Overrided Methods
    protected override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    protected override void OnStay()
    {
        RepulsePlayer();
    }
    #endregion

    private void RepulsePlayer()
    {
        Debug.Log("repulse player()");

        Vector3 dir = (_character.position - transform.position).normalized;

        // apply velocity
        Vector3 vel = dir * _data.RepulsionDistance;
        _character.GetComponent<Rigidbody>().velocity = vel;

        // debug
        GizmosPersistence.DrawPersistentLine(transform.position, transform.position + vel);
    }
}

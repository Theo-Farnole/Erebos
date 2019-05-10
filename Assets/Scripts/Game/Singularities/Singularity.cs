using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singularity : MonoBehaviour
{
    #region Fields
    [SerializeField] private SingularityData _data;
    [Space]
    [SerializeField] private DrawCircle _rangeFeedback;
    [SerializeField] private bool _noOverrideForce = true;

    private bool _isPlayerInRange = false;
    private Transform _player;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        UpdateRangeFeedback();
    }

    void Update()
    {
        IsPlayerInRange();

        if (_isPlayerInRange)
        {
            if (Vector3.Distance(transform.position, _player.position) >= _data.MinRange)
            {
                Vector3 dir = (transform.position - _player.position).normalized;
                _player.GetComponent<Rigidbody>().AddForce(_data.ReactionForce * dir * Time.deltaTime);

                Debug.DrawRay(transform.position, dir * _data.ReactionForce, Color.red);
            }
            // rotate around circle
            else
            {
                

                // reset vel
                _player.GetComponent<Rigidbody>().velocity = Vector3.zero;

                int anglePerSecond = 360;
                float horizontal = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One).x * Time.deltaTime * anglePerSecond;

                Vector3 dir = _player.transform.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                Debug.Log("angle > " + angle);

                angle += horizontal;

                Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                _player.transform.position = transform.position + offset * _data.MinRange;
            }
        }
    }
    #endregion

    void IsPlayerInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _data.Range);

        bool isPlayerInRange = false;

        foreach (Collider c in colliders)
        {
            if (c.CompareTag("Player"))
            {
                isPlayerInRange = true;
                _player = c.transform;
            }
        }

        _isPlayerInRange = isPlayerInRange;

        if (_player != null)
        {
            _player.GetComponent<CharControllerManager>().Attracted = isPlayerInRange;
        }
    }

    public void UpdateRangeFeedback()
    {
        Debug.Log("UpdateRangeFeedback()");

        if (_rangeFeedback == null)
        {
            Debug.LogError(transform.name + " has no range feedback dragged!");
            return;
        }


        _rangeFeedback.xradius = _data.Range;
        _rangeFeedback.yradius = _data.Range;

        _rangeFeedback.UpdateCircle();
    }
}

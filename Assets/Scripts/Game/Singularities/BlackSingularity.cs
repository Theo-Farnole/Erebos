using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * TODO:
 *  > cam zoom
 *  > rotate around
 *  > deactive gravity
 * 
 */
public class BlackSingularity : MonoBehaviour
{
    #region Fields
    [SerializeField] private BlackSingularityData _data;
    [Space]
    [SerializeField] private DrawCircle _rangeFeedback;

    private bool _isPlayerInRange = false;
    private Transform _player;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        _rangeFeedback.xradius = _data.Range;
        _rangeFeedback.yradius = _data.Range;
    }

    void Update()
    {
        IsPlayerInRange();
        
        if (_isPlayerInRange)
        {
            AttractPlayer();
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
            _player.GetComponent<CharController>().Attracted = isPlayerInRange;
        }
    }

    void AttractPlayer()
    {
        Vector3 dir = transform.position - _player.position;
        _player.position += dir * _data.AttractSpeed * Time.deltaTime;
    }
}

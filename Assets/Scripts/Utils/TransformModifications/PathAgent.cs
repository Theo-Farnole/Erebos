using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAgent : MonoBehaviour
{
    #region Fields
    [System.Serializable]
    private class PathPoint
    {
        public Vector3 point;
        public float timeToReachPoint = 1f;
    }

    [SerializeField] private List<PathPoint> _points = new List<PathPoint>();

    private float _timeStart = 0f;
    private int _currentPointIndex = 0;
    private int _oldPointIndex = 0;
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        for (int i = 0; i < _points.Count; i++)
        {
            _points[i].point += transform.position;
        }

        ChangeDirection();

        RespawnHandle d = new RespawnHandle(ResetPosition);
        CharDeath.EventRespawn += d;
    }

    void Update()
    {
        Vector3 currentPoint = _points[_oldPointIndex].point;
        Vector3 targetPoint = _points[_currentPointIndex].point;
        float time = (Time.time - _timeStart) / _points[_currentPointIndex].timeToReachPoint;

        transform.position = Vector3.Lerp(currentPoint, targetPoint, time);

        if (Vector3.Distance(transform.position, _points[_currentPointIndex].point) < 0.1f)
        {
            ChangeDirection();
        }
    }
    #endregion

    void ResetPosition()
    {
        _currentPointIndex = 0;

        transform.position = _points[0].point;
        ChangeDirection();
    }

    private void ChangeDirection()
    {
        _currentPointIndex++;

        if (_currentPointIndex >= _points.Count)
        {
            _currentPointIndex = 0;
        }

        _oldPointIndex = _currentPointIndex - 1;

        if (_oldPointIndex < 0)
        {
            _oldPointIndex = _points.Count - 1;
        }

        _timeStart = Time.time;
    }

#if UNITY_EDITOR
    public void ReversePath()
    {
        _points.Reverse();
    }

    private void OnDrawGizmos()
    {
        foreach (var p in _points)
        {
            if (Application.isPlaying)
            {
                Gizmos.DrawWireSphere(p.point, 0.5f);
            }
            else
            {
                Gizmos.DrawWireSphere(transform.position + p.point, 0.5f);
            }
        }
    }
#endif
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Eclatos : MonoBehaviour
{
    #region Fields
    [SerializeField] private float _timeToEnd = 1f;
    [Space]
    [SerializeField] private Form _startOnForm = Form.Void;
    [Space]
    [SerializeField] private Transform[] _pointsVoid = new Transform[4];
    [SerializeField] private Transform[] _pointsEtheral = new Transform[4];

    private Vector3[] _positionVoid = new Vector3[4];
    private Vector3[] _eulerAngleVoid = new Vector3[4];
    private Vector3[] _positionEthereal = new Vector3[4];
    private Vector3[] _eulerAngleEthereal = new Vector3[4];

    private Transform[] _points = new Transform[4];
    private static readonly float MIN_DISTANCE = 0.1f;
    private static readonly float MIN_ANGLE = 3f;
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        FormHandle d = new FormHandle(OnFormChange);
        CharControllerSingularity.EventForm += d;

        // register transform to points
        for (int i = 0; i < _pointsVoid.Length; i++)
        {
            _positionVoid[i] = _pointsVoid[i].position;
            _eulerAngleVoid[i] = _pointsVoid[i].eulerAngles;
        }

        for (int i = 0; i < _pointsEtheral.Length; i++)
        {
            _positionEthereal[i] = _pointsEtheral[i].position;
            _eulerAngleEthereal[i] = _pointsEtheral[i].eulerAngles;
        }


        // Hide useless transform
        switch (_startOnForm)
        {
            case Form.Normal:
                Debug.LogError(transform.name + " Split On Form can't be \"normal\"! ");
                break;

            case Form.Void:
                _points = _pointsVoid;

                foreach (var t in _pointsEtheral)
                {
                    t.gameObject.SetActive(false);
                }
                break;

            case Form.Ethereal:
                _points = _pointsEtheral;

                foreach (var t in _pointsVoid)
                {
                    t.gameObject.SetActive(false);
                }
                break;
        }
    }
    #endregion

    public void OnFormChange(Form form)
    {
        if (form == Form.Void)
        {
            StopAllCoroutines();
            StartCoroutine(GotoPoints(_positionVoid, _positionEthereal, _eulerAngleVoid, _eulerAngleEthereal));
        }
        else if (form == Form.Ethereal)
        {
            StopAllCoroutines();
            StartCoroutine(GotoPoints(_positionEthereal, _positionVoid, _eulerAngleEthereal, _eulerAngleVoid));
        }
    }

    IEnumerator GotoPoints(Vector3[] position, Vector3[] oldPosition, Vector3[] eulerAngles, Vector3[] oldEulerAngles)
    {
        float startTime = Time.time;

        bool isCompleted;

        do
        {
            isCompleted = true;

            for (int i = 0; i < _points.Length; i++)
            {
                Transform t = _points[i];

                float distancePosition = Vector3.Distance(t.position, position[i]);
                float distanceAngle = Vector3.Distance(t.eulerAngles, eulerAngles[i]);

                // if position is to far from destination...
                if (distancePosition >= MIN_DISTANCE || distanceAngle >= MIN_ANGLE)
                {
                    // ... go to destination.
                    if (distancePosition >= MIN_DISTANCE)
                    {
                        t.position = Vector3.Lerp(oldPosition[i], position[i], (Time.time - startTime) / _timeToEnd);
                    }
                    if (distanceAngle >= MIN_ANGLE)
                    {
                        t.eulerAngles = Vector3.Lerp(oldEulerAngles[i], eulerAngles[i], (Time.time - startTime) / _timeToEnd);
                    }

                    isCompleted = false;

                    if (t.GetComponentInChildren<Collider>() != null)
                    {
                        t.GetComponentInChildren<Collider>().isTrigger = true;
                    }
                }
                else
                {
                    t.position = position[i];
                    t.eulerAngles = eulerAngles[i];

                    if (t.GetComponentInChildren<Collider>())
                    {
                        t.GetComponentInChildren<Collider>().isTrigger = false;
                    }
                }
            }

            yield return new WaitForEndOfFrame();

        } while (isCompleted == false);
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < _positionVoid.Length; i++)
        {
            if (_pointsVoid[i] != null && _pointsEtheral[i] != null)
            {
                switch (i)
                {
                    case 0:
                        Gizmos.color = Color.green;
                        break;

                    case 1:
                        Gizmos.color = Color.red;
                        break;

                    case 2:
                        Gizmos.color = Color.yellow;
                        break;

                    case 3:
                        Gizmos.color = Color.blue;
                        break;
                }
                Gizmos.DrawLine(_pointsVoid[i].position, _pointsEtheral[i].position);
            }
        }
    }
}

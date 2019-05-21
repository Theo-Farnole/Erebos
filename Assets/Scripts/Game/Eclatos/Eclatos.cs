using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Eclatos : MonoBehaviour
{
    #region Fields
    [SerializeField] private float _speedSplit = 8f;
    [Space]
    [SerializeField] private Form _startOnForm = Form.Void;
    [Space]
    [SerializeField] private Transform[] _pointsVoid = new Transform[4];
    [SerializeField] private Transform[] _pointsEtheral = new Transform[4];

    private Vector3[] _positionVoid = new Vector3[4];
    private Vector3[] _positionEthereal = new Vector3[4];

    private Transform[] _points = new Transform[4];
    private static readonly float SPLIT_MIN_DISTANCE = 0.1f;
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
        }

        for (int i = 0; i < _pointsEtheral.Length; i++)
        {
            _positionEthereal[i] = _pointsEtheral[i].position;
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

    public void OnFormChange(object sender, Form form)
    {
        if (form == Form.Void)
        {
            StopAllCoroutines();
            StartCoroutine(GotoPoints(_positionVoid));
        }
        else if (form == Form.Ethereal)
        {
            StopAllCoroutines();
            StartCoroutine(GotoPoints(_positionEthereal));
        }
    }

    IEnumerator GotoPoints(Vector3[] destination)
    {
        Debug.Log("GotoPoints()");

        bool isCompleted;

        do
        {
            isCompleted = true;

            for (int i = 0; i < _points.Length; i++)
            {
                Transform t = _points[i];

                // if position is to far from destination...
                if (Vector3.Distance(t.position, destination[i]) >= SPLIT_MIN_DISTANCE)
                {
                    // ... go to destination.
                    t.position = Vector3.MoveTowards(t.position, destination[i], Time.deltaTime * _speedSplit);

                    isCompleted = false;
                }
                else
                {
                    t.position = destination[i];
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
                Gizmos.color = Color.green;
                Gizmos.DrawLine(_pointsVoid[i].position, _pointsEtheral[i].position);

                //Handles.Label(_pointsVoid[i].position, "V1");
                //Handles.Label(_pointsEtheral[i].position, "E1");
            }
        }
    }
}

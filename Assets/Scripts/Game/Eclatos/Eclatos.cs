using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Eclatos : MonoBehaviour
{
    #region Fields
    [SerializeField] private float _speedSplit = 8f;
    [SerializeField] private Form _splitOnForm = Form.Void;
    [Space]
    [SerializeField] private Transform _topLeftPoint;
    [SerializeField] private Transform _topRightPoint;
    [SerializeField] private Transform _bottomRightPoint;
    [SerializeField] private Transform _bottomLeftPoint;

    private Transform[] _points = new Transform[4];
    private Vector3[] _posSplitted = new Vector3[4];
    private Vector3[] _posGrounded = new Vector3[4];

    private static readonly float SPLIT_MIN_DISTANCE = 0.1f;

    private static readonly Vector3[] GROUNDED_POSITION = new Vector3[4]
    {
        new Vector3(-0.5f, 0.5f),
        new Vector3(0.5f, 0.5f),
        new Vector3(0.5f, -0.5f),
        new Vector3(-0.5f, -0.5f)
    };
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        FormHandle d = new FormHandle(OnFormChange);
        CharControllerSingularity.EventForm += d;

        // SerializeField points to array of Transform
        _points[0] = _topLeftPoint;
        _points[1] = _topRightPoint;
        _points[2] = _bottomRightPoint;
        _points[3] = _bottomLeftPoint;

        // save splitted position
        for (int i = 0; i < _points.Length; i++)
        {
            _posSplitted[i] = _points[i].position;
        }

        // process grounded position
        for (int i = 0; i < _points.Length; i++)
        {
            _posGrounded[i] = transform.position + GROUNDED_POSITION[i];
        }

        if (_splitOnForm == Form.Normal)
        {
            Debug.LogError(transform.name + " Split On Form can't be \"normal\"! ");
        }
    }
    #endregion

    public void OnFormChange(object sender, Form form)
    {
        if (form == _splitOnForm)
        {
            StopAllCoroutines();
            StartCoroutine(GotoPoints(_posSplitted));
        }
        else if (form != Form.Normal && form != _splitOnForm)
        {
            StopAllCoroutines();
            StartCoroutine(GotoPoints(_posGrounded));
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

            Debug.Log("isFullySplited " + isCompleted);

            yield return new WaitForEndOfFrame();

        } while (isCompleted == false);
    }
}

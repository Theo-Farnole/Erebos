using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Eclatos : MonoBehaviour
{
    #region Fields
    [SerializeField] private Transform _topLeftPoint;
    [SerializeField] private Transform _topRightPoint;
    [SerializeField] private Transform _bottomRightPoint;
    [SerializeField] private Transform _bottomLeftPoint;

    private Transform[] _points = new Transform[4];
    private Vector3[] _posSplitted = new Vector3[4];

    private static readonly float SPLIT_SPEED = 3f;
    private static readonly float SPLIT_MIN_DISTANCE = 0.5f;

    private static readonly Vector3[] GROUNDED_POSITION = new Vector3[4]
    {
        new Vector3(-0.5f, 0.5f),
        new Vector3(0.5f, 0.5f),
        new Vector3(-0.5f, 0.5f),
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
    }
    #endregion

    public void OnFormChange(object sender, Form form)
    {
        switch (form)
        {
            case Form.Normal:
                break;

            case Form.Ethereal:
                StopAllCoroutines();
                StartCoroutine(Split());
                break;

            case Form.Void:
                StopAllCoroutines();
                StartCoroutine(Group());
                break;
        }
    }

    IEnumerator Split()
    {
        bool isFullySplited;

        do
        {
            isFullySplited = true;

            for (int i = 0; i < _points.Length; i++)
            {
                Transform t = _points[i];

                if (Vector3.Distance(t.position, _posSplitted[i]) >= SPLIT_MIN_DISTANCE)
                {
                    t.position = Vector3.MoveTowards(t.position, _posSplitted[i], SPLIT_SPEED);
                    isFullySplited = false;
                }
            }

            yield return new WaitForEndOfFrame();

        } while (isFullySplited == false);
    }

    IEnumerator Group()
    {
        yield return new WaitForEndOfFrame();
    }
}

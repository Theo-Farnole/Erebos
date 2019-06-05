using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
public class SingularityEntity : MonoBehaviour
{
    [SerializeField] private BlackSingularity _blackSingularity;
    [SerializeField] private WhiteSingularity _whiteSingularity;
    [SerializeField] private GameObject _normalSingularity;

    void Start()
    {
        FormHandle d = new FormHandle(OnFormChange);
        CharControllerSingularity.EventForm += d;

        OnFormChange(Form.Normal);
    }

    public void OnFormChange(Form form)
    {
        switch (form)
        {
            case Form.Normal:
                _blackSingularity.gameObject.SetActive(false);
                _whiteSingularity.gameObject.SetActive(false);
                _normalSingularity.SetActive(true);
                break;

            case Form.Ethereal:
                _blackSingularity.gameObject.SetActive(false);
                _whiteSingularity.gameObject.SetActive(true);
                _normalSingularity.SetActive(false);
                break;

            case Form.Void:
                _blackSingularity.gameObject.SetActive(true);
                _whiteSingularity.gameObject.SetActive(false);
                _normalSingularity.SetActive(false);
                break;
        }
    }
}

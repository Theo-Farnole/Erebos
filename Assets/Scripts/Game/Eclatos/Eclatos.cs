using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Eclatos : MonoBehaviour
{
    void Start()
    {
        FormHandle d = new FormHandle(OnFormChange);
        CharControllerSingularity.Instance.EventForm += d;
    }

    public void OnFormChange(object sender, Form form)
    {
        Debug.Log("OnHandle of " + form);

        switch (form)
        {
            case Form.Normal:
                break;

            case Form.Ethereal:
                break;

            case Form.Void:
                break;
        }
    }
}

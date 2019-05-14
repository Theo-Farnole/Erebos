using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class SingularityEntity : MonoBehaviour
{
    [SerializeField] private BlackSingularity _blackSingularity;
    [SerializeField] private WhiteSingularity _whiteSingularity;

    void Update()
    {
        switch (CharControllerSingularity.form)
        {
            case CharControllerSingularity.Form.Normal:
                _blackSingularity.gameObject.SetActive(false);
                _whiteSingularity.gameObject.SetActive(false);
                break;

            case CharControllerSingularity.Form.Ethereal:
                _blackSingularity.gameObject.SetActive(false);
                _whiteSingularity.gameObject.SetActive(true);
                break;

            case CharControllerSingularity.Form.Void:
                _blackSingularity.gameObject.SetActive(true);
                _whiteSingularity.gameObject.SetActive(false);
                break;
        }
    }
}

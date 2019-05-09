using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSingularity : MonoBehaviour
{
    #region Fields
    [SerializeField] private BlackSingularityData _data;
    [Space]
    [SerializeField] private DrawCircle _rangeFeedback;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        _rangeFeedback.xradius = _data.Range;
        _rangeFeedback.yradius = _data.Range;
    }
    #endregion
}

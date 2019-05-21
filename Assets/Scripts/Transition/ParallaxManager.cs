using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : Singleton<ParallaxManager>
{
    [SerializeField] private AnimationCurve _speedOnX;
    [SerializeField] private AnimationCurve _speedOnY;

    void Update()
    {
        float horizontal = _speedOnX.Evaluate(Time.timeSinceLevelLoad);
        float vertical = _speedOnY.Evaluate(Time.timeSinceLevelLoad);

        transform.position += new Vector3(horizontal, vertical, 0);
    }
}

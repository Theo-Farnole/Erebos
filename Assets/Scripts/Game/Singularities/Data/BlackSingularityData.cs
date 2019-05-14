using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Erebos/Black Singularity")]
public class BlackSingularityData : ScriptableObject
{
    [SerializeField] private float _timeToReachCenter = 3f;
    [SerializeField] private float _range = 3f;

    public float TimeToReachCenter { get => _timeToReachCenter; }
    public float Range { get => _range; }

#if UNITY_EDITOR
    void OnValidate()
    {
        var singularities = GameObject.FindObjectsOfType<BlackSingularity>();

        foreach (var s in singularities)
        {
            s.UpdateRangeFeedback();
        }
    }
#endif
}

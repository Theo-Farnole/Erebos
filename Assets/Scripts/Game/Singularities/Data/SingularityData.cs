using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Erebos/Singularity")]
public class SingularityData : ScriptableObject
{
    [SerializeField] private float _range = 8f;
    [SerializeField] private float _minRange = 3f;
    [Space]
    [SerializeField] private float _reactionForce = 20000f;
    [SerializeField] private int _anglePerSecond = 360;

    public float Range { get => _range; }
    public float MinRange { get => _minRange; }
    public float ReactionForce { get => _reactionForce; }
    public int AnglePerSecond { get => _anglePerSecond; }

#if UNITY_EDITOR
    void OnValidate()
    {
        var singularities = GameObject.FindObjectsOfType<Singularity>();

        foreach (var s in singularities)
        {
            s.UpdateRangeFeedback();
        } 
    }
#endif
}


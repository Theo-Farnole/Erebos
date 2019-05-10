using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Erebos/Singularity")]
public class SingularityData : ScriptableObject
{
    [SerializeField] private float _range = 3f;
    [SerializeField] private float _reactionForce = 3f;

    public float Range { get => _range; }
    public float ReactionForce { get => _reactionForce; }

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


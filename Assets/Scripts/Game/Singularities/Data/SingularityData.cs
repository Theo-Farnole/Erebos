using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Erebos/Singularity")]
public class SingularityData : ScriptableObject
{
    [SerializeField] private float _range = 8f;
    [SerializeField] private float _minRange = 3f;
    [Space]
    [SerializeField] private float _reactionForce = 2f;
    [SerializeField] private int _anglePerSecond = 360;    

    public float Range { get => _range; }
    public float MinRange { get => _minRange; }
    public float ReactionForce { get => _reactionForce * 1e5f; }
    public int AnglePerSecond { get => _anglePerSecond; }
}


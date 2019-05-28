using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Erebos/Black Singularity")]
public class BlackSingularityData : ScriptableObject
{
    [SerializeField] private float _timeToReachCenter = 3f;
    [SerializeField] private float _radius = 3f;
    [SerializeField] private float _characterRotateRadius = 3f;

    public float TimeToReachCenter { get => _timeToReachCenter; }
    public float Radius { get => _radius; }
    public float CharacterRotateRadius { get => _characterRotateRadius;}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Erebos/Black Singularity")]
public class BlackSingularityData : ScriptableObject
{
    [SerializeField] private float _range = 3f;
    [SerializeField] private float _attractSpeed = 3f;

    public float Range { get => _range; }
    public float AttractSpeed { get => _attractSpeed; }
}

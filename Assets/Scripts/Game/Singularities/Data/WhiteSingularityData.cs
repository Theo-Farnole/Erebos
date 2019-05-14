using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Erebos/White Singularity")]
public class WhiteSingularityData : ScriptableObject
{
    [SerializeField] private float _repulsionDistance = 3f;

    public float RepulsionDistance { get => _repulsionDistance; }
}

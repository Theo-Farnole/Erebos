using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Erebos/CharController")]
public class CharControllerData : ScriptableObject
{
    [SerializeField] private float _speed = 3f;
    [Header("Jumps settings")]
    [Range(0, 1)]
    [SerializeField] private float _airControl = 1f;
    [SerializeField] private float _firstJumpForce = 400f;
    [SerializeField] private float _secondJumpForce = 200f;

    public float Speed { get => _speed; }
    public float AirControl { get => _airControl; }
    public float FirstJumpForce { get => _firstJumpForce; }
    public float SecondJumpForce { get => _secondJumpForce; }
}

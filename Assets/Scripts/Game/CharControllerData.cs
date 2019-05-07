using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Erebos/CharController")]
public class CharControllerData : ScriptableObject
{
    [SerializeField] private float _speed = 3f;
    [Header("Jumps settings")]
    [SerializeField] private float _firstJumpForce = 400f;
    [SerializeField] private float _secondJumpForce = 200f;
    [SerializeField] private float _stickedJumpForce = 200f;
    [Space]
    [Range(0, 1)]
    [SerializeField] private float _airControl = 1f;
    

    public float Speed { get => _speed; }
    public float AirControl { get => _airControl; }
    public float FirstJumpForce { get => _firstJumpForce; }
    public float SecondJumpForce { get => _secondJumpForce; }
    public float StickedJumpForce { get => _stickedJumpForce; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Erebos/CharController")]
public class CharControllerData : ScriptableObject
{
    [Header("Run settings")]
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _maxVelocityOnX = 10f;
    [Header("Jumps settings")]
    [SerializeField] private float _firstJumpForce = 400f;
    [SerializeField] private float _secondJumpForce = 200f;
    [SerializeField] private float _stickedJumpForce = 200f;
    [Space]
    [Range(0, 1)]
    [SerializeField] private float _airControlMultiplier = 1f;
    

    public float Speed { get => _speed; }
    public float MaxVelocityOnX { get => _maxVelocityOnX;  }

    public float AirControlMultiplier { get => _airControlMultiplier; }
    public float FirstJumpForce { get => _firstJumpForce; }
    public float SecondJumpForce { get => _secondJumpForce; }
    public float StickedJumpForce { get => _stickedJumpForce; }
}

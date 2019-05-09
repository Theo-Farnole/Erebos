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
    [SerializeField] private float _firstJumpHeight = 4;
    [SerializeField] private float _secondJumpHeight = 8;
    [Space]
    [SerializeField] private float _stickedJumpForce = 20;
    [Space]
    [SerializeField] private float _fallMultiplier = 2.5f;
    [SerializeField] private float _airControlSpeed = 1f;
    

    public float Speed { get => _speed; }
    public float MaxVelocityOnX { get => _maxVelocityOnX;  }

    public float StickedJumpForce { get => _stickedJumpForce; }

    public float AirControlSpeed { get => _airControlSpeed; }
    public float FallMultiplier { get => _fallMultiplier; }
    public float FirstJumpHeight { get => _firstJumpHeight;}
    public float SecondJumpHeight { get => _secondJumpHeight; }
}

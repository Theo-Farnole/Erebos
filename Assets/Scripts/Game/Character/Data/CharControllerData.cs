using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Erebos/CharController")]
public class CharControllerData : ScriptableObject
{
    [Header("Run settings")]
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _maxVelocityOnX = 10f;
    [Space]
    [SerializeField] private float _turnSpeed = 60f;
    [Header("Jumps settings")]
    [SerializeField] private float _jumpHeight = 4;
    [SerializeField] private float _dashDistance = 8;
    [SerializeField] private float _dashTime = 8;
    [Space]
    [SerializeField] private float _stickedJumpForce = 20;
    [SerializeField, Range(0, 90)] private float _stickedJumpAngle = 45f;
    [Space]
    [SerializeField] private float _fallMultiplier = 2.5f;
    [SerializeField] private float _airControlSpeed = 1f;
    

    public float Speed { get => _speed; }
    public float MaxVelocityOnX { get => _maxVelocityOnX;  }

    public float StickedJumpForce { get => _stickedJumpForce; }

    public float AirControlSpeed { get => _airControlSpeed; }
    public float FallMultiplier { get => _fallMultiplier; }
    public float JumpHeight { get => _jumpHeight;}
    public float DashDistance { get => _dashDistance; }
    public float StickedJumpAngle { get => _stickedJumpAngle; }
    public float DashTime { get => _dashTime;  }
    public float TurnSpeed { get => _turnSpeed; }
}

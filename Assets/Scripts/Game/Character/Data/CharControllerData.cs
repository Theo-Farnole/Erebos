using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Erebos/CharController")]
public class CharControllerData : ScriptableObject
{
    [Header("Walk")]
    [SerializeField] private float _maxVelocityOnX = 10f;
    [Space]
    [SerializeField] private float _turnSpeed = 60f;
    [Header("Jump")]
    [SerializeField] private float _jumpHeight = 4;
    [Header("Dash")]
    [SerializeField] private float _dashDistance = 8;
    [SerializeField] private float _dashTime = 8;
    [Space]
    [SerializeField] private float _dashInertia = 1;
    [Header("Sticking")]
    [SerializeField] private float _stickedJumpForce = 20;
    [SerializeField, Range(0, 90)] private float _stickedJumpAngle = 45f;
    [Header("Air")]
    [SerializeField] private float _fallMultiplier = 2.5f;
    [SerializeField] private float _airControlSpeed = 1f;
    

    public float MaxVelocityOnX { get => _maxVelocityOnX;  }
    public float TurnSpeed { get => _turnSpeed; }

    public float JumpHeight { get => _jumpHeight;}

    public float DashDistance { get => _dashDistance; }
    public float DashTime { get => _dashTime;  }
    public float DashInertia { get => _dashInertia; }

    public float StickedJumpForce { get => _stickedJumpForce; }
    public float StickedJumpAngle { get => _stickedJumpAngle; }

    public float AirControlSpeed { get => _airControlSpeed; }
    public float FallMultiplier { get => _fallMultiplier; }
}

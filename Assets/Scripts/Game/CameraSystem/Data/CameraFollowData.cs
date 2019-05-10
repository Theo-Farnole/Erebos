using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Erebos/Camera Follow")]
public class CameraFollowData : ScriptableObject
{
    [SerializeField] private float _maxOffset = 8f;
    [SerializeField] private float _speed = 3f;

    public float MaxOffset { get => _maxOffset; }
    public float Speed { get => _speed;  }
}

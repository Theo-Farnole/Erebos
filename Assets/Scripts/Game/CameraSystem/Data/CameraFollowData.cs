using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Erebos/Camera Follow")]
public class CameraFollowData : ScriptableObject
{
    [SerializeField] private Vector3 _speed = Vector3.one * 5f;
    [Header("Player's Inputs")]
    [SerializeField, Range(0, 100)] private float _inputPercentOffset = 8f;
    [SerializeField] private float _inputSpeed = 8f;
    [Header("Camera Position")]
    [SerializeField, Range(0, 100)] private float _deltaFromCenterWidthPercent = 30f;
    [Space]
    [SerializeField, Range(0, 100)] private float _panicLineMaxY = 70;
    [SerializeField, Range(0, 100)] private float _panicLineMinY = 30;
    [Header("Zoom")]
    [SerializeField] private float _zoomOut = 15;
    [SerializeField] private float _zoomIn = 15;

    public Vector3 Speed { get => _speed; }

    public float InputPercentOffset { get => _inputPercentOffset / 100f; }
    public float InputSpeed { get => _inputSpeed; }

    public float DeltaFromCenterWidthPercent { get => _deltaFromCenterWidthPercent / 100f; }
    public float PanicLineMaxY { get => _panicLineMaxY / 100;}
    public float PanicLineMinY { get => _panicLineMinY / 100;}

    public float ZoomOut { get => _zoomOut; }
    public float ZoomIn { get => _zoomIn; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform transformToFollow;

    void Update()
    {
        transform.position = transformToFollow.position;
        transform.rotation = transformToFollow.rotation;
    }
}

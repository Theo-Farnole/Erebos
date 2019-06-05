using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform transformToFollow;

    void Update()
    {
        if (transformToFollow == null)
            return;

        transform.position = transformToFollow.position;
        transform.rotation = transformToFollow.rotation;
    }
}

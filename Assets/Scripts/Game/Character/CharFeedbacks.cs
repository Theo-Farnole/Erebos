using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharFeedbacks : Singleton<CharFeedbacks>
{
    [SerializeField] private ParticleSystem _prefabJumpPS;

    public void PlayJumpPS()
    {
        GameObject obj = Instantiate(_prefabJumpPS, transform.position, Quaternion.Euler(new Vector3(36.68f, transform.eulerAngles.y))).gameObject;
        Destroy(obj, 3f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharFeedbacks : Singleton<CharFeedbacks>
{
    #region Fields
    [SerializeField] private GameObject _prefabTrail;
    [SerializeField] private ParticleSystem _prefabJumpPS;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        var trail = Instantiate(_prefabTrail, transform.position, Quaternion.identity);
        trail.GetComponent<FollowTransform>().transformToFollow = transform;
    }
    #endregion

    public void PlayJumpPS()
    {
        GameObject obj = Instantiate(_prefabJumpPS, transform.position, Quaternion.Euler(new Vector3(36.68f, transform.eulerAngles.y))).gameObject;
        obj.transform.parent = transform;
        Destroy(obj, 3f);
    }
}

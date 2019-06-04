using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharFeedbacks : Singleton<CharFeedbacks>
{
    #region Fields
    [SerializeField] private GameObject _model;
    [Space]
    [SerializeField] private GameObject _prefabTrail;
    [SerializeField] private GameObject _prefabJumpPS;
    [SerializeField] private GameObject _prefabDeathPS;
    [Header("Dash")]
    [SerializeField] private GameObject _prefabBurstDash;
    [SerializeField] private GameObject _prefabHeadDash;
    [SerializeField] private GameObject _prefabEndDash;
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
        GameObject obj = Instantiate(_prefabJumpPS, transform.position, Quaternion.Euler(new Vector3(36.68f, transform.eulerAngles.y)));
        obj.transform.parent = transform;
        Destroy(obj, 3f);
    }

    public void PlayDeathPS()
    {
        GameObject obj = Instantiate(_prefabDeathPS, transform.position, Quaternion.Euler(new Vector3(36.68f, transform.eulerAngles.y)));
        Destroy(obj, 3f);
    }

    public void PlayDashSequence(float angle)
    {
        Debug.Log("Dash Sequence!");

        Vector3 rotation = Quaternion.identity.eulerAngles + new Vector3(0, 0, angle);
        Instantiate(_prefabBurstDash, transform.position, Quaternion.Euler(rotation));
    }
}

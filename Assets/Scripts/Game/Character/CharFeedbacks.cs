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
    [Header("Death")]
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

        DeathHandle d1 = new DeathHandle(PlayDeathPS);
        CharDeath.EventDeath += d1;

        DeathHandle d2 = new DeathHandle((object sender) => _model.SetActive(false));
        CharDeath.EventDeath += d2;

        RespawnHandle d3 = new RespawnHandle((object sender) => _model.SetActive(true));
        CharDeath.EventRespawn += d3;
    }
    #endregion

    public void PlayJumpPS()
    {
        Instantiate(_prefabJumpPS, transform.position, Quaternion.Euler(new Vector3(36.68f, transform.eulerAngles.y)));
    }

    public void PlayDeathPS(object sender = null)
    {
        Instantiate(_prefabDeathPS, transform.position, Quaternion.Euler(new Vector3(36.68f, transform.eulerAngles.y)));
    }

    public void PlayDashSequence(float angle)
    {
        Debug.Log("Dash Sequence!");

        Vector3 rotation = Quaternion.identity.eulerAngles + new Vector3(0, 0, angle);
        Instantiate(_prefabBurstDash, transform.position, Quaternion.Euler(rotation));

        _model.SetActive(false);

        GameObject obj = Instantiate(_prefabHeadDash, transform.position, Quaternion.Euler(rotation));
        obj.transform.parent = transform;
    }

    public void StopDashSequence()
    {
        Instantiate(_prefabEndDash, transform.position + Vector3.back * 3f, Quaternion.identity);

        StartCoroutine(CustomDelay.ExecuteAfterTime(0.2f, () => _model.SetActive(true)));
    }
}

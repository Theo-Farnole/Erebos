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
    [SerializeField] private GameObject _prefabRespawnPS;
    [Header("Dash")]
    [SerializeField] private GameObject _prefabBurstDash;
    [SerializeField] private GameObject _prefabHeadDash;
    [SerializeField] private GameObject _prefabEndDash;
    [Header("Form")]
    [SerializeField] private GameObject _prefabBlackForm;
    [SerializeField] private GameObject _prefabWhiteForm;

    private Coroutine _coroutineDashSequence = null;
    private CharControllerSingularity _charControllerSingularity = null;

    private bool _isDashing = false;
    private bool _isDead = false;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        var trail = Instantiate(_prefabTrail, transform.position, Quaternion.identity);
        trail.GetComponent<FollowTransform>().transformToFollow = transform;

        DeathHandle d1 = new DeathHandle(PlayDeath);
        CharDeath.EventDeath += d1;

        RespawnHandle d2 = new RespawnHandle(PlayRespawn);
        CharDeath.EventRespawn += d2;

        FormHandle d3 = new FormHandle(PlayFormChange);
        CharControllerSingularity.EventForm += d3;

        _charControllerSingularity = GetComponent<CharControllerSingularity>();
    }

    void Update()
    {
        bool isInBlackSingularity = CharControllerManager.Instance.Attracted && _charControllerSingularity.Form == Form.Void;

        bool shouldBeActive = !(_isDashing || _isDead || isInBlackSingularity);

        _model.SetActive(shouldBeActive);    
    }
    #endregion

    public void PlayJumpPS()
    {
        Instantiate(_prefabJumpPS, transform.position, Quaternion.identity);
    }

    void PlayFormChange(object sender, Form form)
    {
        switch (form)
        {
            case Form.Ethereal:
                Instantiate(_prefabWhiteForm, transform.position, Quaternion.identity);
                break;

            case Form.Void:
                Instantiate(_prefabBlackForm, transform.position, Quaternion.identity);
                break;
        }
    }

    #region Death & Respawn
    void PlayDeath(object sender)
    {
        _isDead = true;
        Instantiate(_prefabDeathPS, transform.position, Quaternion.identity);
    }

    void PlayRespawn(object sender)
    {
        _isDead = false;
        Instantiate(_prefabRespawnPS, transform.position, Quaternion.identity);
    }
    #endregion

    #region Dash Sequence
    public void PlayDashSequence(float angle)
    {
        Debug.Log("Dash Sequence!");

        Vector3 rotation = Quaternion.identity.eulerAngles + new Vector3(0, 0, angle);
        Instantiate(_prefabBurstDash, transform.position, Quaternion.Euler(rotation));

        GameObject obj = Instantiate(_prefabHeadDash, transform.position, Quaternion.Euler(rotation));
        obj.transform.parent = transform;

        _isDashing = true;
    }

    public void StopDashSequence()
    {
        Instantiate(_prefabEndDash, transform.position + Vector3.back * 3f, Quaternion.identity);

        _isDashing = false;
    }
    #endregion
}

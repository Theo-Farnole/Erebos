using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharFeedbacks : Singleton<CharFeedbacks>
{
    #region Fields
    [SerializeField] private GameObject _model;
    [SerializeField] private GameObject _blackMask;
    [Space]
    [SerializeField] private GameObject _prefabJumpPS;
    [SerializeField] private GameObject _prefabBlackMaskTrail;
    [Header("Form")]
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    [Space]
    [SerializeField] private Material _materialBlackForm;
    [SerializeField] private Material _materialWhiteForm;
    [Space]
    [SerializeField] private GameObject _prefabBlackFormPS;
    [SerializeField] private GameObject _prefabWhiteFormPS;
    [Header("Death")]
    [SerializeField] private GameObject _prefabDeathPS;
    [SerializeField] private GameObject _prefabRespawnPS;
    [Header("Dash")]
    [SerializeField] private GameObject _prefabBurstDash;
    [SerializeField] private GameObject _prefabHeadDash;
    [SerializeField] private GameObject _prefabEndDash;

    private bool _isDashing = false;
    private GameObject _blackMaskTrail = null;

    // cached variables
    private CharControllerSingularity _charControllerSingularity = null;
    private int _hashCDDash = -1;
    #endregion

    #region Properties
    public GameObject BlackMask { get => _blackMask; }
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        _charControllerSingularity = GetComponent<CharControllerSingularity>();
    }

    void Start()
    {
        DeathHandle d1 = new DeathHandle(PlayDeath);
        CharDeath.EventDeath += d1;

        RespawnHandle d2 = new RespawnHandle(PlayRespawn);
        CharDeath.EventRespawn += d2;

        FormHandle d3 = new FormHandle(PlayFormChange);
        CharControllerSingularity.EventForm += d3;

        _blackMaskTrail = Instantiate(_prefabBlackMaskTrail, transform.position, Quaternion.identity);
        _blackMaskTrail.GetComponent<FollowTransform>().transformToFollow = transform;
    }

    void Update()
    {
        bool isInBlackSingularity = CharControllerManager.Instance.Attracted && _charControllerSingularity.Form == Form.Void;

        bool shouldBeActive = !(_isDashing || CharDeath.isDead || isInBlackSingularity);

        _model.SetActive(shouldBeActive);
        _blackMask.SetActive(isInBlackSingularity);
        _blackMaskTrail.SetActive(isInBlackSingularity);
    }
    #endregion

    public void PlayJumpPS()
    {
        Instantiate(_prefabJumpPS, transform.position, Quaternion.identity);
    }

    public void UpdateFormMaterial(bool hasDash)
    {
        int propertyValue = hasDash ? 0 : 1;

        _meshRenderer.material.SetFloat("_CDDash", propertyValue);
    }

    void PlayFormChange(Form form)
    {
        switch (form)
        {
            case Form.Ethereal:
                var obj = Instantiate(_prefabWhiteFormPS, transform.position, Quaternion.identity);
                obj.transform.parent = transform;

                _meshRenderer.material = _materialWhiteForm;
                break;

            case Form.Void:
                obj = Instantiate(_prefabBlackFormPS, transform.position, Quaternion.identity);
                obj.transform.parent = transform;

                _meshRenderer.material = _materialBlackForm;
                break;
        }
    }

    #region Death & Respawn
    void PlayDeath()
    {
        Instantiate(_prefabDeathPS, transform.position, Quaternion.Euler(-90, 0, 0));
    }

    void PlayRespawn()
    {
        Instantiate(_prefabRespawnPS, transform.position, Quaternion.Euler(-90, 0, 0));
    }
    #endregion

    #region Dash Sequence
    public void PlayDashSequence(float angle)
    {
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

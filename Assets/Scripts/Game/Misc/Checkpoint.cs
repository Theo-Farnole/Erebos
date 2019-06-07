using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject _braseroFX;
    [SerializeField] private GameObject _auraLight;

    private bool _hasBeenTriggered = false;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        SetActiveBrasero(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_hasBeenTriggered)
                return;

            _hasBeenTriggered = true;
            SetActiveBrasero(true);
        }
    }
    #endregion

    private void SetActiveBrasero(bool active)
    {
        _braseroFX.SetActive(active);
        _auraLight.SetActive(active);

        if (active)
        {
            AudioManager.Instance.PlaySoundGeneral(SoundGeneral.Checkpoint);
        }
    }
}

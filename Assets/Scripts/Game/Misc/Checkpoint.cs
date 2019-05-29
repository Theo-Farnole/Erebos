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
        _braseroFX.SetActive(false);
        _auraLight.SetActive(false);
    }

    public void ActiveBrasero()
    {
        if (_hasBeenTriggered)
            return;

        _braseroFX.SetActive(true);
        _auraLight.SetActive(true);

        AudioManager.Instance.PlaySoundGeneral(SoundGeneral.Checkpoint);
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSingularity : MonoBehaviour
{
    #region Fields
    protected Transform _character;
    #endregion

    #region MonoBehaviour Callbacks
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _character = other.transform;
            OnEnter();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnStay();
            CameraFollow.Instance.SmoothCenterOnCharacter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _character.GetComponent<CharControllerManager>().Attracted = false;
    }
    #endregion

    protected abstract void OnEnter();
    protected abstract void OnStay();
}

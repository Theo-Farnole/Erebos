using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSingularity : MonoBehaviour
{
    #region Fields
    protected Transform _character;
    #endregion

    #region MonoBehaviour Callbacks
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _character = other.transform;
            _character.GetComponent<CharControllerManager>().Attracted = true;
            OnStay();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _character.GetComponent<CharControllerManager>().Attracted = false;
            OnExit();
        }
    }
    #endregion

    protected abstract void OnStay();
    protected abstract void OnExit();

}

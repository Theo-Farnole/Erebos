﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFeather : MonoBehaviour
{
    #region Fields
    [SerializeField] private Form _unlockForm;
    [SerializeField] private GameObject _prefabDestroyPS;

    private bool _hasBeenTriggered = false;
    #endregion

    #region MonoBehaviour Callbacks
    private void OnTriggerEnter(Collider other)
    {
        if (_hasBeenTriggered)
            return;

        if (other.CompareTag("Player"))
        {
            _hasBeenTriggered = true;

            CharControllerSingularity charControllerSingularity = other.GetComponent<CharControllerSingularity>();

            switch (_unlockForm)
            {
                case Form.Normal:
                    Debug.LogWarning(transform.name + " Tutorial Feather can't unlock normal form.");
                    break;

                case Form.Ethereal:
                    Debug.Log("Ethereal form unlocked!");
                    charControllerSingularity.canGotoEtheral = true;
                    break;

                case Form.Void:
                    Debug.Log("Void form unlocked!");
                    charControllerSingularity.canGotoVoid = true;
                    break;
            }

            Instantiate(_prefabDestroyPS, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    #endregion
}
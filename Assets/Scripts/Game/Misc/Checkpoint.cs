using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject _braseroFX;
    [SerializeField] private GameObject _auraLight;

    void Awake()
    {
        _braseroFX.SetActive(false);
        _auraLight.SetActive(false);
    }

    public void ActiveBrasero()
    {
        _braseroFX.SetActive(true);
        _auraLight.SetActive(true);
    }
}

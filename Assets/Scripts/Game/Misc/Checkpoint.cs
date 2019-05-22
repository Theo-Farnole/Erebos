using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject _braseroFX;

    void Awake()
    {
        _braseroFX.SetActive(false);
    }

    public void ActiveBrasero()
    {
        _braseroFX.SetActive(true);
    }
}

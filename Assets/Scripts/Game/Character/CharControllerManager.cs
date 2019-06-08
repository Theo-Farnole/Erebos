using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControllerManager : Singleton<CharControllerManager>
{
    #region Fields
    private bool _attracted = false;

    private CharController _charController = null;
    private CharControllerSingularity _charControllerSingularity = null;
    private Rigidbody _charRigidbody = null;
    #endregion

    #region Properties
    public bool Attracted
    {
        get
        {
            return _attracted;
        }

        set
        {
            _attracted = value;

            _charController.enabled = !_attracted;
            GetComponent<Rigidbody>().useGravity = !_attracted;

            if (_attracted)
            {
                _charController.ResetMovements();                
            }
            else
            {
                _charControllerSingularity.isRotatingAroundSingularity = false;
            }
        }
    }
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        _charController = GetComponent<CharController>();
        _charControllerSingularity = GetComponent<CharControllerSingularity>();
        _charRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // on death, set attracted to false
        DeathHandle d = new DeathHandle(() =>
        {
            Attracted = false;
            GameState.CurrentDeathCount++;
        });
        CharDeath.EventDeath += d;

        // on form change, set parent to null
        FormHandle dd = new FormHandle((Form form) =>
        {
            transform.parent = null;
            Attracted = false;
        });
        CharControllerSingularity.EventForm += dd;
    }

    void Update()
    {
        transform.eulerAngles = Vector3.zero;
    }
    #endregion
}

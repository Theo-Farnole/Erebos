using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControllerManager : Singleton<CharControllerManager>
{
    private bool _attracted = false;

    public bool Attracted
    {
        get
        {
            return _attracted;
        }

        set
        {
            _attracted = value;

            GetComponent<CharController>().enabled = !_attracted;
            GetComponent<Rigidbody>().useGravity = !_attracted;

            if (_attracted)
            {
                GetComponent<CharController>().ResetMovements();
            }
        }
    }

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        // on death, set attracted to false
        DeathHandle d = new DeathHandle((object sender) =>
        {
            Attracted = false;
        });
        CharDeath.EventDeath += d;
    }
    #endregion
}

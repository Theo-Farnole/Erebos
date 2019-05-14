using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControllerManager : MonoBehaviour
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
                GetComponent<CharController>().AttractReset();
            }
        }
    }
}

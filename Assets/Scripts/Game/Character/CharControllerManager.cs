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
            GetComponent<CharController>().enabled = !_attracted;
            GetComponent<Rigidbody>().useGravity = !_attracted;

            _attracted = value;
        }
    }
}

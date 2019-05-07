using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasLink : MonoBehaviour
{
    public readonly float MAX_VALIDATION_DISTANCE = 0.1f;

    #region Fields
    [SerializeField] private Camera _firstCamera;
    [SerializeField] private Camera _gotoCamera;
    [Space]
    [SerializeField] private float _speed = 3F;

    private bool _isTriggered = false;
    #endregion

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter with " + other.name);
        if (_isTriggered)
            return;

        _isTriggered = true;
        StartCoroutine(MoveView());
    }

    IEnumerator MoveView()
    {
        Debug.Log("MoveView()");
        
        // current camera goto other camera position ...
        while (Vector3.Distance(_firstCamera.transform.position, _gotoCamera.transform.position) > MAX_VALIDATION_DISTANCE)
        {
            _firstCamera.transform.position = Vector3.MoveTowards(_firstCamera.transform.position, _gotoCamera.transform.position, _speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        // ... then disable current camera and enable other camera.
        _firstCamera.gameObject.SetActive(false);
        _gotoCamera.gameObject.SetActive(true);

        _gotoCamera.transform.position = _firstCamera.transform.position;
        _gotoCamera.GetComponent<CameraFollow>().UpdateOffset();
    }
}

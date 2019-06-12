using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    #region Fields
    [SerializeField] private float _timeToScaleDown = 3f;
    [SerializeField] private GameObject _prefabDestroyPS = null;

    private bool _hasBeenTriggered = false;
    private Vector3 _startingScale = Vector3.one;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        _startingScale = transform.localScale;
    }

    void OnTriggerEnter(Collider other)
    {
        if (_hasBeenTriggered)
            return;

        if (other.CompareTag("Player"))
        {
            _hasBeenTriggered = true;

            if (GameManager.Instance == null)
            {
                Debug.LogError("No GameManager in scene!");
                return;
            }

            GameManager.Instance.GatherCollectible(this);

            // play fx and destro collectible
            CharFeedbacks.Instance.PlayJumpPS();
            StartCoroutine(ScaleDown());
        }
    }
    #endregion

    IEnumerator ScaleDown()
    {
        float time = Time.time;

        while (transform.localScale != Vector3.zero)
        {
            transform.localScale = Vector3.Lerp(Vector3.one, Vector2.zero, (Time.time - time) / _timeToScaleDown);

            yield return new WaitForEndOfFrame();
        }

        Instantiate(_prefabDestroyPS, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    public void ResetScale()
    {
        _hasBeenTriggered = false;

        transform.localScale = _startingScale;
        gameObject.SetActive(true);
    }
}

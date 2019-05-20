using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    #region Fields
    [SerializeField] private float _timeToScaleDown = 3f;
    [SerializeField] private GameObject _prefabDestroyPS = null;

    private bool _hasBeenTriggered = false;
    #endregion

    #region MonoBehaviour Callbacks
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

            GameManager.Instance.AddCollectible();

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
        Destroy(gameObject);
    }
}

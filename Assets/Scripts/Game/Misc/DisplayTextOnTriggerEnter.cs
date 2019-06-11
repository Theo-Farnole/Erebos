using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTextOnTriggerEnter : MonoBehaviour
{
    #region Fields
    [SerializeField] private string _key = string.Empty;
    [Space]
    [SerializeField] private float _fadeInTime = 1;
    [SerializeField] private float _idleTime = 1;
    [SerializeField] private float _fadeOutTime = 1;

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

            var text = UIManager.Instance.TextIndicator;

            text.text = _key;
            text.GetComponent<TranslateText>().DynamicTextUpdate();

            text.Fade(FadeType.FadeIn, _fadeInTime);
            this.ExecuteAfterTime(_idleTime, () => text.Fade(FadeType.FadeOut, _fadeOutTime));
        }
    }
    #endregion
}

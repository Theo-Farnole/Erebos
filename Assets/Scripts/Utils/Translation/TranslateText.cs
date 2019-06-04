using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public sealed class TranslateText : MonoBehaviour
{
    #region Fields
    private string _key = string.Empty;
    private TextMeshProUGUI _text;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _key = _key != string.Empty ? _key : _text.text;

        UpdateText();
    }

    void Start()
    {
        LanguageHandle d = new LanguageHandle(UpdateText);
        UIMenuManager.EventLanguageChangement += d;
    }
    #endregion

    void UpdateText()
    {
        Debug.Log("event receved");
        _text.text = Translation.Get(_key);
    }
}
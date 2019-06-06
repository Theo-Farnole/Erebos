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
    }

    void Start()
    {
        LanguageHandle d = new LanguageHandle(UpdateText);
        UIMenuManager.EventLanguageChangement += d;

        _key = _text.text;

        UpdateText();
    }
    #endregion

    /// <summary>
    /// Update key with text then, update text.
    /// </summary>
    public void DynamicTextUpdate()
    {
        _key = _text.text;
        UpdateText();
    }

    void UpdateText()
    {
        _text.text = Translation.Get(_key);
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public sealed class TranslateText : MonoBehaviour
{
    private string _key = null;

    void Start()
    {
        var text = GetComponent<TextMeshProUGUI>();
        text.text = Translation.Get(_key != string.Empty ? _key : text.text);
        Destroy(this);
    }
}
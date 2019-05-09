using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public sealed class TranslateText : MonoBehaviour
{
    [SerializeField]
    private string m_Key = null;

    private void Start()
    {
        var text = GetComponent<TextMeshProUGUI>();
        text.text = Translation.Get(m_Key != string.Empty ? m_Key : text.text);
        Destroy(this);
    }
}
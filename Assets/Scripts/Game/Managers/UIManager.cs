using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    #region Fields
    [SerializeField] private TextMeshProUGUI _textCollectible;
    #endregion

    public void UpdateTextCollectible()
    {
        _textCollectible.text = GameManager.Instance.CurrentCollectibles.ToString();
    }
}

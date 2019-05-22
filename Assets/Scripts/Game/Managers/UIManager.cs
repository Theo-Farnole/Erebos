using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    #region Fields
    [Header("InGame UI")]
    [SerializeField] private TextMeshProUGUI _textCollectible;
    [Header ("Pause UI")]
    [SerializeField] private GameObject _panelPause;
    [Space]
    [SerializeField] private TextMeshProUGUI[] _textSpeedRunsTimes  = new TextMeshProUGUI[3];
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        _panelPause.SetActive(false);
    }
    #endregion

    public void UpdateTextCollectible()
    {
        _textCollectible.text = GameManager.Instance.CurrentCollectibles.ToString();
    }

    public void UpdatePanelPause()
    {
        GameState.SaveSpeedRunTime();

        bool isInPause = (Time.timeScale == 0);

        if (isInPause)
        {
            for (int i = 0; i < GameState.speedrunTime.Length; i++)
            {
                float t = GameState.speedrunTime[i];
                _textSpeedRunsTimes[i].text = string.Format("{0:0.0} s", t); ;
            }
        }
 
        _panelPause.SetActive(isInPause);
    }
}

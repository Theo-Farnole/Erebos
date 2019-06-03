using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    #region Fields
    [Header("-- InGame UI ")]
    [SerializeField] private TextMeshProUGUI _textCollectible;
    [Header("-- Pause UI ")]
    [SerializeField] private GameObject _panelPause;
    [Space]
    [Header("-- Pause UI - Timers")]
    [SerializeField] private TextMeshProUGUI[] _textSpeedRunsTimes = new TextMeshProUGUI[3];
    [SerializeField] private TextMeshProUGUI[] _textDeathCount = new TextMeshProUGUI[3];
    [Header("-- Pause UI - Buttons")]
    [SerializeField] private Button _buttonRestart;
    [SerializeField] private Button _buttonQuitPause;
    [Header("-- Feathers")]
    [SerializeField] private GameObject _panelWhiteFeather;
    [SerializeField] private GameObject _panelBlackFeather;
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        _panelPause.SetActive(false);

        _buttonRestart.onClick.AddListener(() => GameManager.Instance.RestartCheckpoint());
        _buttonQuitPause.onClick.AddListener(() => UpdatePanelPause());

        UpdateTextCollectible();
    }
    #endregion

    public void UpdateTextCollectible()
    {
        _textCollectible.text = GameManager.Instance.CurrentCollectibles + " / " + GameState.CurrentMaxCollectibles;
    }

    public void UpdatePanelPause()
    {
        GameState.CurrentSpeedrunTime = Time.timeSinceLevelLoad;

        bool isInPause = (Time.timeScale == 0);

        if (isInPause)
        {
            for (int i = 0; i < GameState.speedrunTime.Length; i++)
            {
                float t = GameState.speedrunTime[i];

                float minutes = (t / 60);
                float seconds = (t % 60);

                _textSpeedRunsTimes[i].text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }

            for (int i = 0; i < GameState.speedrunTime.Length; i++)
            {
                float d = GameState.deathCount[i];
                _textDeathCount[i].text = d.ToString();
            }
        }

        _panelPause.SetActive(isInPause);
    }
}

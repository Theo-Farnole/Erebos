using GamepadInput;
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
    [SerializeField] private Image _imageCollectible;
    [Space]
    [SerializeField] private float _fadeoutTime = 2.3f;
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

        _panelWhiteFeather.SetActive(false);
        _panelBlackFeather.SetActive(false);
    }

    void Update()
    {
        // deactivate if on tutorial feather
        if (GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.One) ||
        GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.One))
        {
            if (_panelWhiteFeather.activeSelf)
            {
                _panelWhiteFeather.SetActive(false);
                Time.timeScale = 1;
            }

            if (_panelBlackFeather.activeSelf)
            {
                _panelBlackFeather.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
    #endregion

    #region Collectibles
    public void StartDisplayCollectiblesText()
    {
        _textCollectible.text = GameManager.Instance.CurrentCollectibles + " / " + GameState.CurrentMaxCollectibles;

        _textCollectible.Fade(FadeType.FadeOut, _fadeoutTime);
        _imageCollectible.Fade(FadeType.FadeOut, _fadeoutTime);
    }
    #endregion

    public void DisplayUnlockFormPanel(Form unlockedForm)
    {
        switch (unlockedForm)
        {
            case Form.Ethereal:
                _panelWhiteFeather.SetActive(true);
                Time.timeScale = 0;
                break;

            case Form.Void:
                _panelBlackFeather.SetActive(true);
                Time.timeScale = 0;
                break;
        }
    }

    public void UpdatePanelPause()
    {
        GameState.UpdateCurrentSpeedrunTime();
        bool isInPause = (Time.timeScale == 0);

        if (isInPause)
        {
            for (int i = 0; i < GameState.speedrunTime.Length; i++)
            {
                float t = GameState.speedrunTime[i];

                int min = Mathf.FloorToInt(t / 60);
                int sec = Mathf.FloorToInt(t % 60);

                string txt = string.Format(min.ToString("00") + ":" + sec.ToString("00"));

                _textSpeedRunsTimes[i].text = txt;
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

using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{

    #region Fields
    [Header("-- InGame UI ")]
    [SerializeField] private TextMeshProUGUI _textCollectible;
    [SerializeField] private Image _imageCollectible;
    [Space]
    [SerializeField] private float _collectiblesFadeoutTime = 2.3f;
    [Header("-- Pause UI ")]
    [SerializeField] private GameObject _panelPause;
    [Space]
    [Header("-- Pause UI - Timers")]
    [SerializeField] private TextMeshProUGUI[] _textSpeedRunsTimes = new TextMeshProUGUI[3];
    [SerializeField] private TextMeshProUGUI[] _textDeathCount = new TextMeshProUGUI[3];
    [Header("-- Pause UI - Buttons")]
    [SerializeField] private Button _buttonContinue;
    [SerializeField] private Button _buttonStats;
    [SerializeField] private Button _buttonRestart;
    [SerializeField] private Button _buttonQuit;
    [Header("-- Feathers")]
    [SerializeField] private float _tutorialFadeInTime = 1f;
    [Space]
    [SerializeField] private GameObject _panelWhiteFeather;
    [SerializeField] private GameObject _panelBlackFeather;
    [Space]
    [SerializeField] private TextMeshProUGUI _textWhiteFeather;
    [SerializeField] private TextMeshProUGUI _textBlackFeather;
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        _panelPause.SetActive(false);

        // pause button
        _buttonContinue.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            UpdatePanelPause();
        });
        _buttonRestart.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });

        //_buttonStats.onClick.AddListener(() => UpdatePanelPause());
        _buttonQuit.onClick.AddListener(Application.Quit);

        _panelWhiteFeather.SetActive(false);
        _panelBlackFeather.SetActive(false);

        _textCollectible.gameObject.SetActive(false);
        _imageCollectible.gameObject.SetActive(false);
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

        _textCollectible.gameObject.SetActive(true);
        _imageCollectible.gameObject.SetActive(true);

        _textCollectible.Fade(FadeType.FadeOut, _collectiblesFadeoutTime);
        _imageCollectible.Fade(FadeType.FadeOut, _collectiblesFadeoutTime);
    }
    #endregion

    public void DisplayUnlockFormPanel(Form unlockedForm)
    {
        switch (unlockedForm)
        {
            case Form.Ethereal:
                Time.timeScale = 0;

                _panelWhiteFeather.SetActive(true);

                _panelWhiteFeather.GetComponent<Image>().Fade(FadeType.FadeIn, _tutorialFadeInTime);
                _textWhiteFeather.Fade(FadeType.FadeIn, _tutorialFadeInTime);
                break;

            case Form.Void:
                Time.timeScale = 0;

                _panelBlackFeather.SetActive(true);

                _panelBlackFeather.GetComponent<Image>().Fade(FadeType.FadeIn, _tutorialFadeInTime);
                _textBlackFeather.Fade(FadeType.FadeIn, _tutorialFadeInTime);
                break;
        }
    }

    public void UpdatePanelPause()
    {
        GameState.UpdateCurrentSpeedrunTime();
        bool isInPause = (Time.timeScale == 0);

        _panelPause.SetActive(isInPause);
    }
}

using Erebos.Inputs;
using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] private GameObject _panelControls;
    [Space]
    [Header("-- Pause UI - Timers")]
    [SerializeField] private TextMeshProUGUI[] _textSpeedRunsTimes = new TextMeshProUGUI[3];
    [SerializeField] private TextMeshProUGUI[] _textDeathCount = new TextMeshProUGUI[3];
    [Header("-- Pause UI - Buttons")]
    [SerializeField] private Button _buttonContinue;
    [SerializeField] private Button _buttonControls;
    [SerializeField] private Button _buttonRestart;
    [SerializeField] private Button _buttonQuit;
    [Header("-- Pause UI - Contrôles")]
    [SerializeField] private Scrollbar _verticalScrollbar;
    [Header("-- Tutorials")]
    [Header("-- Tutorials - IG text")]
    [SerializeField] private TextMeshProUGUI _textIndicator; // used outside of this script

    [Header("-- Tutorials - Feather")]
    [SerializeField] private float _tutorialFadeInTime = 1f;
    [Space]
    [SerializeField] private GameObject _panelWhiteFeather;
    [SerializeField] private TextMeshProUGUI _textWhiteFeather;
    [SerializeField] private GameObject _imageWhiteFeather;
    [Space]
    [SerializeField] private GameObject _panelBlackFeather;
    [SerializeField] private TextMeshProUGUI _textBlackFeather;
    [SerializeField] private GameObject _imageBlackFeather;

    private bool _isRestarting = false;
    private bool _isReturningToMainMenu = false;
    #endregion

    #region Properties
    public TextMeshProUGUI TextIndicator { get => _textIndicator; }
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        _panelPause.SetActive(false);
        _panelControls.SetActive(false);

        // pause button
        _buttonContinue.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            UpdatePanelPause();
        });

        _buttonRestart.onClick.AddListener(() =>
        {
            if (_isRestarting)
                return;

            _isRestarting = true;

            Time.timeScale = 1;
            Initiate.Fade(SceneManager.GetActiveScene().name, Color.black, 1);

            UpdatePanelPause();
        });

        _buttonControls.onClick.AddListener(() =>
        {
            _panelPause.SetActive(false);
            _panelControls.SetActive(true);

            EventSystem.current.SetSelectedGameObject(_verticalScrollbar.gameObject);
            
        });

        _buttonQuit.onClick.AddListener(() =>
        {
            if (_isReturningToMainMenu)
                return;

            _isReturningToMainMenu = true;

            Time.timeScale = 1;
            Initiate.Fade("SC_main_menu", Color.black, 1);

            UpdatePanelPause();
        });

        _textCollectible.gameObject.SetActive(false);
        _imageCollectible.gameObject.SetActive(false);

        _panelWhiteFeather.SetActive(false);
        _panelBlackFeather.SetActive(false);
        _imageWhiteFeather.SetActive(false);
        _imageBlackFeather.SetActive(false);
    }

    void Update()
    {
        if (_panelWhiteFeather.activeSelf && _panelWhiteFeather.GetComponent<Image>().color.a == 1)
        {
            _imageWhiteFeather.SetActive(true);

            if (InputProxy.SkipTutorial)
            {
                _panelWhiteFeather.SetActive(false);
                Time.timeScale = 1;
            }
        }

        if (_panelBlackFeather.activeSelf && _panelBlackFeather.GetComponent<Image>().color.a == 1)
        {
            _imageBlackFeather.SetActive(true);

            if (InputProxy.SkipTutorial)
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
        _panelControls.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_buttonContinue.gameObject);
    }
}

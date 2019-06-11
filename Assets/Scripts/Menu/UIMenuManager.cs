using Erebos.Inputs;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void LanguageHandle();

public class UIMenuManager : MonoBehaviour
{
    #region Enums
    private enum PanelType
    {
        MainMenu,
        Options,
        Credits
    };
    #endregion

    #region Fields
    #region SerializeFields
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private GameObject _background;
    [Header("Panels")]
    [SerializeField] private GameObject _panelMainMenu;
    [SerializeField] private GameObject _panelOptionsGeneral;
    [SerializeField] private GameObject _panelCredits;
    [Header("Main Menu")]
    [SerializeField] private Button _buttonPlay;
    [SerializeField] private Button _buttonOptions;
    [SerializeField] private Button _buttonCredits;
    [SerializeField] private Button _buttonQuit;
    [Header("Options Panel")]
    [SerializeField] private Selector _selectorLanguage;
    [Space]
    [SerializeField] private Toggle _toggleVSync;
    [SerializeField] private Selector _selectorFullscreen;
    [Header("Return label")]
    [SerializeField] private GameObject _labelReturn;
    #endregion

    #region Public static fields
    public static LanguageHandle EventLanguageChangement;
    #endregion

    #region Privates Fields
    private PanelType _currentPanel = PanelType.MainMenu;
    private AsyncOperation _ao;
    private bool _playedHasBeenPressed = false;
    #endregion
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        LoadSettings();
        DisplayPanel(PanelType.MainMenu);
    }

    void Start()
    {
        // doesn't work in Awake()
        _ao = SceneManager.LoadSceneAsync(SceneState.Tutorial.ToScene());
        _ao.allowSceneActivation = false;

        // main menu
        _buttonPlay.onClick.AddListener(LoadTutorial);  // load tutorial
        _buttonOptions.onClick.AddListener(() => DisplayPanel(PanelType.Options));
        _buttonCredits.onClick.AddListener(() => DisplayPanel(PanelType.Credits));
        _buttonQuit.onClick.AddListener(Application.Quit);

        // selector
        _selectorLanguage.onValueChanged.AddListener(SelectorLanguageChanged);

        // set audio for every buttons
        foreach (Button b in FindObjectsOfType<Button>())
        {
            b.onClick.AddListener(() => AudioManager.Instance.PlaySoundUI(SoundUI.Button));
        }
    }

    void Update()
    {
        if (InputProxy.Menu.Back)
        {
            ReturnButtonPressed();
        }
    }

    void OnDestroy()
    {
        EventLanguageChangement = null;
    }
    #endregion

    void LoadTutorial()
    {
        if (_playedHasBeenPressed)
            return;

        _playedHasBeenPressed = true;
        Initiate.Fade(SceneState.Tutorial.ToScene(), Color.black, 1f);
    }

    void DisplayPanel(PanelType panel)
    {
        _currentPanel = panel;

        _panelMainMenu.SetActive(false);
        _panelOptionsGeneral.SetActive(false);
        _panelCredits.SetActive(false);

        _labelReturn.gameObject.SetActive(true);
        _background.gameObject.SetActive(false);

        switch (panel)
        {
            case PanelType.MainMenu:
                _panelMainMenu.SetActive(true);
                _eventSystem.SetSelectedGameObject(_buttonPlay.gameObject);

                _labelReturn.gameObject.SetActive(false);
                _background.gameObject.SetActive(true);
                break;

            case PanelType.Options:
                _panelOptionsGeneral.SetActive(true);
                _eventSystem.SetSelectedGameObject(_selectorLanguage.gameObject);
                break;

            case PanelType.Credits:
                _panelCredits.SetActive(true);
                break;
        }
    }

    void ReturnButtonPressed()
    {
        switch (_currentPanel)
        {
            case PanelType.Credits:
            case PanelType.Options:
                DisplayPanel(PanelType.MainMenu);
                SaveSettings();
                break;
        }
    }

    void SelectorLanguageChanged()
    {
        SaveSettings();
        Translation.ResetTranslations();

        EventLanguageChangement?.Invoke();
    }

    #region Load/Save Settings Methods
    void LoadSettings()
    {
        _selectorLanguage.Value = SaveSystem.OptionsData.Language;

        _toggleVSync.isOn = SaveSystem.OptionsData.enableVSync;
        _selectorFullscreen.Value = SaveSystem.OptionsData.FullScreenValue;
    }

    void SaveSettings()
    {
        SaveSystem.OptionsData.Language = _selectorLanguage.Value;

        SaveSystem.OptionsData.enableVSync = _toggleVSync.isOn;
        SaveSystem.OptionsData.FullScreenValue = _selectorFullscreen.Value;

        SaveSystem.Save();
    }
    #endregion
}

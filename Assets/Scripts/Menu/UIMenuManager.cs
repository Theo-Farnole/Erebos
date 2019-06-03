using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// NOTE:
// add async load of tutorial
public class UIMenuManager : MonoBehaviour
{
    #region Enums
    private enum PanelType
    {
        MainMenu,
        OptionsGeneral,
        OptionsSound,
        OptionsQuality,
        Credits
    };
    #endregion

    [Header("Panels")]
    [SerializeField] private GameObject _panelMainMenu;
    [SerializeField] private GameObject _panelOptionsGeneral;
    [SerializeField] private GameObject _panelOptionsSound;
    [SerializeField] private GameObject _panelOptionsQuality;
    [SerializeField] private GameObject _panelCredits;
    [Header("Main Menu")]
    [SerializeField] private Button _buttonPlay;
    [SerializeField] private Button _buttonOptions;
    [SerializeField] private Button _buttonCredits;
    [SerializeField] private Button _buttonQuit;
    [Header("Quality Panel")]
    [SerializeField] private Button _buttonQuality;
    [SerializeField] private Button _buttonSound;
    [Space]
    [SerializeField] private Toggle _toggleVSync;
    [SerializeField] private TMP_Dropdown _dropdownFullscreen;
    [Header("Returns buttons")]
    [SerializeField] private List<Button> _buttonsReturn = new List<Button>();

    private PanelType _currentPanel = PanelType.MainMenu;

    void Awake()
    {
        LoadVideoSettings();
        DisplayPanel(PanelType.MainMenu);

        // main menu
        _buttonPlay.onClick.AddListener(() => SceneManager.LoadScene(SceneState.Tutorial.ToScene()));  // load tutorial
        _buttonOptions.onClick.AddListener(() => DisplayPanel(PanelType.OptionsGeneral));
        _buttonCredits.onClick.AddListener(() => DisplayPanel(PanelType.Credits));
        _buttonQuit.onClick.AddListener(() => Application.Quit());

        // quality panel
        _buttonQuality.onClick.AddListener(() => DisplayPanel(PanelType.OptionsQuality));
        _buttonSound.onClick.AddListener(() => DisplayPanel(PanelType.OptionsSound));

        // return buttons
        foreach (Button b in _buttonsReturn)
        {
            b.onClick.AddListener(() => ReturnButtonPressed());
        }
    }

    void DisplayPanel(PanelType panel)
    {
        _currentPanel = panel;

        _panelMainMenu.SetActive(false);
        _panelOptionsGeneral.SetActive(false);
        _panelOptionsSound.SetActive(false);
        _panelOptionsQuality.SetActive(false);
        _panelCredits.SetActive(false);

        switch (panel)
        {
            case PanelType.MainMenu:
                _panelMainMenu.SetActive(true);
                break;

            case PanelType.OptionsGeneral:
                _panelOptionsGeneral.SetActive(true);
                break;

            case PanelType.OptionsSound:
                _panelOptionsSound.SetActive(true);
                break;

            case PanelType.OptionsQuality:
                _panelOptionsQuality.SetActive(true);
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
            case PanelType.OptionsGeneral:
                DisplayPanel(PanelType.MainMenu);
                break;

            case PanelType.OptionsSound:
            case PanelType.OptionsQuality:
                DisplayPanel(PanelType.OptionsGeneral);
                SaveVideoSettings();
                break;
        }
    }

    #region Load/Save Settings Methods
    void LoadVideoSettings()
    {
        _toggleVSync.isOn = SaveSystem.OptionsData.enableVSync;
        _dropdownFullscreen.SetValueWithoutNotify(SaveSystem.OptionsData.FullScreenValue);
    }

    void SaveVideoSettings()
    {
        SaveSystem.OptionsData.enableVSync = _toggleVSync.isOn;
        SaveSystem.OptionsData.FullScreenValue = _dropdownFullscreen.value;

        SaveSystem.Save();
    }
    #endregion
}

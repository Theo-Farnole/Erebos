using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISettingsManager : MonoBehaviour
{
    private enum UIState { InGame, Paused, Video, Audio };

    #region Fields
    [Header("Pause Panels")]
    [SerializeField] private GameObject _panelGeneral;
    [SerializeField] private GameObject _panelVideo;
    [SerializeField] private GameObject _panelAudio;
    [Header("Buttons")]
    [SerializeField] private Button _buttonClose;
    [SerializeField] private Button _buttonRestart;
    [SerializeField] private Button _buttonVideo;
    [SerializeField] private Button _buttonAudio;
    [Header("Video Settings")]
    [SerializeField] private Toggle _toggleVSync;
    [SerializeField] private Toggle _toggleDisplayFPS;

    private UIState _uiState = UIState.Paused;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        _uiState = UIState.Paused;
        ActiveCurrentPanel();

        // setting listeners to buttons
        _buttonClose.onClick.AddListener(() => CloseButtonPressed());
        _buttonRestart.onClick.AddListener(() => RestartButtonPressed());
        _buttonVideo.onClick.AddListener(() => VideoButtonPressed());
        _buttonAudio.onClick.AddListener(() => AudioButtonPressed());

        // settings listeners to toggles
        //_toggleVSync
    }
    #endregion

    #region Buttons Methods
    void RestartButtonPressed()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    void VideoButtonPressed()
    {            
        _uiState = UIState.Video;
        ActiveCurrentPanel();
    }

    void AudioButtonPressed()
    {
        _uiState = UIState.Audio;
        ActiveCurrentPanel();
    }

    void CloseButtonPressed()
    {
        switch (_uiState)
        {
            case UIState.Paused:
                Debug.LogWarning("Return to InGame not implemented.");
                break;

            case UIState.Video:
            case UIState.Audio:
                SaveVideoSettings();

                _uiState = UIState.Paused;
                ActiveCurrentPanel();
                break;
        }
    }
    #endregion

    #region Load/Save Settings Methods
    void LoadVideoSettings()
    {
        _toggleVSync.isOn = SaveSystem.optionsData.enableVSync;
        //_toggleDisplayFPS.isOn = SaveSystem.optionsData.fullscreenMode;
    }

    void SaveVideoSettings()
    {
        SaveSystem.optionsData.enableVSync = _toggleVSync.isOn;
        //SaveSystem.optionsData.fullscreenMode = _toggleDisplayFPS.isOn;

        SaveSystem.Save();
    }
    #endregion

    void ActiveCurrentPanel()
    {
        _panelGeneral.SetActive(false);
        _panelVideo.SetActive(false);
        _panelAudio.SetActive(false);

        switch (_uiState)
        {
            case UIState.Paused:
                _panelGeneral.SetActive(true);
                break;

            case UIState.Video:
                LoadVideoSettings();
                _panelVideo.SetActive(true);
                break;

            case UIState.Audio:
                _panelAudio.SetActive(true);
                break;
        }
    }
}

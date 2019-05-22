using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    #region Fields
    private int _currentCollectibles = 0;
    #endregion

    #region Properties
    public int CurrentCollectibles { get => _currentCollectibles; }
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        SaveSystem.Load();

        ApplySavedSettings();
    }

    void Update()
    {
        if (GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.One))
        {
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
            UIManager.Instance.UpdatePanelPause();
        }
    }
    #endregion

    public void ApplySavedSettings()
    {
        int vSyncCount = SaveSystem.optionsData.enableVSync ? 2 : 0;
        QualitySettings.vSyncCount = vSyncCount;
    }

    public void AddCollectible()
    {
        _currentCollectibles++;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateTextCollectible();
        }
    }

#if UNITY_EDITOR
    void OnGUI()
    {
        if (UIManager.Instance != null)
            return;

        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 80;
        style.normal.textColor = Color.red;

        GUI.Label(rect, "collectible(s) x " + CurrentCollectibles, style);
    }
#endif
}

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
        GameState.CurrentMaxCollectibles = FindObjectsOfType<Collectible>().Length;
    }

    void Start()
    {
        var d1 = new RespawnHandle(FreeCollectibles);
        CharDeath.EventRespawn += d1;
    }

    void Update()
    {
        if (!Initiate.AreWeFading && GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.One))
        {
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
            UIManager.Instance.UpdatePanelPause();
        }
    }
    #endregion

    #region Collectibles Methods
    public void GatherCollectible()
    {
        _currentCollectibles++;

        AudioManager.Instance.PlaySoundGeneral(SoundGeneral.Collectible);

        UIManager.Instance.StartGatherCollectible(true);
    }

    public void ValidateCollectibles()
    {
        if (_currentCollectibles == 0)
            return;

        GameState.CurrentCollectibles += _currentCollectibles;
        _currentCollectibles = 0;

        UIManager.Instance.StartDisplayCollectiblesText();
        AudioManager.Instance.PlaySoundGeneral(SoundGeneral.Collectible);
    }

    void FreeCollectibles()
    {
        _currentCollectibles = 0;

        UIManager.Instance.StartGatherCollectible(false);
    }

    #endregion

    public void RestartCheckpoint()
    {
        CharControllerManager.Instance.GetComponent<CharDeath>().Death();
        Time.timeScale = 1;
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

        GUI.Label(rect, CurrentCollectibles + " / " + GameState.CurrentMaxCollectibles, style);
    }
#endif
}

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

        _textCollectible.gameObject.SetActive(false);
        _imageCollectible.gameObject.SetActive(false);

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
        Debug.Log("FadeOut");

        // update text
        _textCollectible.text = GameManager.Instance.CurrentCollectibles + " / " + GameState.CurrentMaxCollectibles;

        StartCoroutine(FadeOut(new Graphic[] { _textCollectible, _imageCollectible }, _fadeoutTime));
    }


    IEnumerator FadeOut(Graphic[] _graphics, float timeToFadout)
    {
        // active graphics & set their alpha to 1
        foreach (Graphic g in _graphics)
        {
            g.gameObject.SetActive(true);

            var color = g.color;
            color.a = 1;
            g.color = color;
        }

        bool everythingFadeOut;
        float startingTime = Time.unscaledTime;


        do
        {
            everythingFadeOut = true;
            float deltaTime = Time.unscaledTime - startingTime;
            float newAlpha = Mathf.Lerp(1, 0, deltaTime / timeToFadout);

            if (newAlpha > 0f)
            {
                everythingFadeOut = false;
            }

            foreach (Graphic g in _graphics)
            {
                // change color
                Color color = g.color;
                color.a = newAlpha;
                g.color = color;

                Debug.Log(g.name + " has an alpha of " + color.a);
            }

            yield return new WaitForEndOfFrame();
        }
        while (!everythingFadeOut);

        // deactive for 
        foreach (Graphic g in _graphics)
        {
            g.gameObject.SetActive(false);
        }

        Debug.Log("FadeOut ended");
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

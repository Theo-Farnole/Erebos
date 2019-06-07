using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIEndManager : MonoBehaviour
{
    [SerializeField] private Graphic _buttonGotoMenu;
    [Space]
    [SerializeField] private TextMeshProUGUI _textTime;
    [SerializeField] private TextMeshProUGUI _textDeaths;
    [SerializeField] private TextMeshProUGUI _textCollectibles;

    void Start()
    {
        float t = GameState.speedrunTime.Sum();

        int min = Mathf.FloorToInt(t / 60);
        int sec = Mathf.FloorToInt(t % 60);
        string time = string.Format(min.ToString("00") + ":" + sec.ToString("00"));

        _textTime.text = time;
        _textDeaths.text = GameState.deathCount.Sum().ToString();
        _textCollectibles.text = GameState.Collectibles.Sum().ToString() + " / " + GameState.maxCollectibles.Sum().ToString();
    }

    void Update()
    {
        if (GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.One) && _buttonGotoMenu.color.a == 1)
        {
            ;
            SceneManager.LoadScene("SC_main_menu");
        }
    }
}

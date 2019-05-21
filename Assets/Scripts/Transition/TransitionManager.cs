using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private TextMeshProUGUI _textPressAnyKey;
    private AsyncOperation ao;
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        Debug.Log("TransionManager Start()");
        _textPressAnyKey.gameObject.SetActive(false);

        LoadScene();
    }

    void Update()
    {
        ProcessInput();
    }
    #endregion

    void LoadScene()
    {
        string sceneToLoad = GameState.state.ToScene();

        ao = SceneManager.LoadSceneAsync(sceneToLoad);
        ao.allowSceneActivation = false;
    }

    void ProcessInput()
    {
        if (ao.progress >= 0.9f)
        {
            _textPressAnyKey.gameObject.SetActive(true);

            if (GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.One))
            {
                ao.allowSceneActivation = true;
            }
        }
    }
}

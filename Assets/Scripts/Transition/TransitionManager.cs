using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : Singleton<TransitionManager>
{
    #region Fields
    [Header("Panel linkage")]
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _image;
    [Header("Transitions data")]
    [SerializeField] private List<Transition> _transitionsZoneOne = new List<Transition>();
    [SerializeField] private List<Transition> _transitionsZoneTwo = new List<Transition>();
    [SerializeField] private List<Transition> _transitionsEnd = new List<Transition>();

    private Dictionary<SceneState, List<Transition>> _transitions = new Dictionary<SceneState, List<Transition>>();
    private AsyncOperation ao;

    private int _currentTransitionIndex= -1;
    private Transition _currentTransition;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        _transitions.Add(SceneState.ZoneOne, _transitionsZoneOne);
        _transitions.Add(SceneState.ZoneTwo, _transitionsZoneTwo);
        _transitions.Add(SceneState.End, _transitionsEnd);
    }

    void Start()
    {
        // load scene except if it's the end of the game
        if (GameState.currentScene != SceneState.End)
        {
            string sceneToLoad = GameState.currentScene.ToScene();

            ao = SceneManager.LoadSceneAsync(sceneToLoad);
            ao.allowSceneActivation = false;
        }

        ChangeTransition();
    }

    void Update()
    {
        if (GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.One) || Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Wola!");

            if (_currentTransition != null)
            {
                _currentTransition.ChangeDialogue();
            }
        }
    }
    #endregion

    public void ChangeTransition()
    {
        _currentTransitionIndex++;

        if (_currentTransitionIndex < _transitions[GameState.currentScene].Count)
        {
            if (GameState.currentScene == SceneState.Tutorial)
            {
                Debug.LogError("Can't load transition for tutorial!");
            }
            else
            {
                _currentTransition = _transitions[GameState.currentScene][_currentTransitionIndex];
                _currentTransition.LoadVignette(ref _image, ref _text);
            }
        }
        else
        {
            if (ao != null)
            {
                ao.allowSceneActivation = true;
            }
        }
    }

}

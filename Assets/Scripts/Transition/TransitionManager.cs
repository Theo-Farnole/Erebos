using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : Singleton<TransitionManager>
{
    #region Fields
    [SerializeField] private bool _override;
    [SerializeField] private SceneState _overrideState;
    [Space]
    [SerializeField] private List<Transition> _transitionsZoneOne = new List<Transition>();
    [SerializeField] private List<Transition> _transitionsZoneTwo = new List<Transition>();
    [SerializeField] private List<Transition> _transitionsEnd = new List<Transition>();

    private Dictionary<SceneState, List<Transition>> _transitions = new Dictionary<SceneState, List<Transition>>();
    private AsyncOperation ao;

    private int _currentTransition = -1;
    #endregion

    #region Properties
    private SceneState CurrentScene
    {
        get
        {
#if UNITY_EDITOR
            if (_override)
            {
                return _overrideState;
            }
#endif

            return GameState.currentScene;
        }
    }
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
        // load scene
        if (GameState.currentScene != SceneState.End)
        {
            string sceneToLoad = GameState.currentScene.ToScene();

            ao = SceneManager.LoadSceneAsync(sceneToLoad);
            ao.allowSceneActivation = false;
        }

        ChangeTransition();
    }
    #endregion

    public void ChangeTransition()
    {
        _currentTransition++;

        if (_currentTransition < _transitions[CurrentScene].Count)
        {
            if (CurrentScene == SceneState.Tutorial)
            {
                Debug.LogError("Can't load transition for tutorial!");
            }
            else
            {
                _transitions[CurrentScene][_currentTransition].LoadVignette();

                if (_currentTransition - 1 >= 0)
                {
                    _transitions[CurrentScene][_currentTransition - 1].UnloadVignette();
                }
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

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Transition : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _text;
    [Space]
    [SerializeField] private int _cinematicIndex = 0;

    private int _currentDialogue = -1;
    private string _dialogueKey;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        _panel.SetActive(false);

        _dialogueKey = "cinematics." + _cinematicIndex + ".";
    }

    void Update()
    {
        if (_panel.activeSelf && Input.GetKeyDown(KeyCode.A))
        {
            ChangeDialogue();
        }
    }
    #endregion

    public void LoadVignette()
    {
        _panel.SetActive(true);
        ChangeDialogue();
    }

    void UnloadVignette()
    {
        _panel.SetActive(false);
        TransitionManager.Instance.ChangeTransition();
    }

    void ChangeDialogue()
    {
        Debug.Log("ChangeDialogue! " + transform.name);

        _currentDialogue++;

        string key = _dialogueKey + _currentDialogue;
        string text = Translation.Get(key);

        // if dialogue is ended
        if (text == key)
        {
            UnloadVignette();
        }
        else
        {
            _text.text = text;
        }
    }
}
